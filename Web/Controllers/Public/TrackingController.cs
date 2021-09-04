using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using AutoMapper.QueryableExtensions;
using Core.Model;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Web.Dto.Auth;
using Web.Dto.Home;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Web.Controllers.Public
{
    [ApiController]
    [Route("/api/tracking")]
    public class TrackingController : Controller
    {
        private const int TrackPeriod = 1000;
        private readonly IMemoryCache _authCache;
        private readonly IConfiguration _configuration;

        private readonly ModelContext _context;
        private readonly IConfigurationProvider _mapperConfig;
        private IDictionary<string, DateTime> _onlineAnons = new Dictionary<string, DateTime>();

        private IDictionary<string, DateTime> _onlineMembers = new Dictionary<string, DateTime>();

        public TrackingController(ModelContext context, IConfigurationProvider mapperConfig,
            IConfiguration configuration, IMemoryCache cache)
        {
            _context = context;
            _mapperConfig = mapperConfig;
            _configuration = configuration;
            _authCache = cache;
        }

        [Route("statistics")]
        [HttpGet]
        [ResponseCache(Duration = TrackPeriod)]
        public JsonResult Statistic()
        {
            var totalUsers = _context.Users.Count();
            var totalPosts = _context.Posts.Count();
            var totalThreads = _context.Threads.Count();
            return new JsonResult(new ForumStatistic
                {
                    TotalMembers = totalUsers,
                    TotalPosts = totalPosts,
                    TotalThreads = totalThreads,
                    OnlineMembers = 5,
                    OnlineAnonymous = 122
                }
            );
        }

        [Route("active-threads")]
        [HttpGet]
        [ResponseCache(Duration = TrackPeriod)]
        public JsonResult ActiveThreads()
        {
            var query = _context.Threads
                .ProjectTo<ThreadView>(_mapperConfig)
                .OrderBy("LastActivityAt desc, Priority")
                .Take(10);
            return new JsonResult(query);
        }

        [Route("active-forums")]
        [HttpGet]
        public JsonResult ActiveForums([FromQuery(Name = "categories[]")] List<string> categories)
        {
            // get 8 of each categories
            var query = _context.Forums
                .Where(x => categories.Contains(x.Category.Slug));

            if (!HttpContext.User.IsInRole("Admin"))
                query = query.Where(x => (short)x.ForumAccess < (short)AccessMode.Internal);

            return new JsonResult(
                query.GroupBy(c => c.CategoryId)
                    .SelectMany(g => g.OrderByDescending(x => x.Priority).Take(5))
                    .Include("Category")
                    .ProjectTo<ForumView>(_mapperConfig)
            );
        }


        // Handle tracking user by open logging api. user
        // For a forum application, real time is not a critical requirement (~15 mins delay is okay)
        // We will migrate to SignalR later on (if we have enough time)
        [Route("logs")]
        [HttpPost]
        public ActionResult Track()
        {
            var cookies = Request.Cookies[_configuration["JwtConfig:Refresh:Cookies"]];
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            if (string.IsNullOrWhiteSpace(cookies))
            {
                _onlineAnons[ip] = DateTime.Now;
                RemoveInactiveEntry();
                return new OkResult();
            }

            if (!_authCache.TryGetValue(cookies, out JwtToken value))
            {
                _onlineAnons[ip] = DateTime.Now;
                RemoveInactiveEntry();
                return new OkResult();
            }

            _onlineMembers[value.User.Username] = DateTime.Now;
            _onlineAnons.Remove(ip);
            RemoveInactiveEntry();
            return new OkResult();
        }

        private void RemoveInactiveEntry()
        {
            var expireTime = DateTime.Now.AddSeconds(-100 - TrackPeriod);
            _onlineMembers = _onlineMembers.Where(kv => kv.Value < expireTime)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            _onlineAnons = _onlineAnons.Where(kv => kv.Value < expireTime)
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}