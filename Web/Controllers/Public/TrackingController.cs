using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Web.Dto.Auth;
using Web.Dto.Home;

namespace Web.Controllers.Public
{
    [ApiController]
    [Route("/api/tracking")]
    public class TrackingController : Controller
    {
        private readonly IMemoryCache _authCache;
        private readonly IConfiguration _configuration;

        private readonly ModelContext _context;
        private readonly ForumService _forumService;
        private readonly OnlineUserCache _onlineUserCache;
        private readonly ThreadService _threadService;

        public TrackingController(ModelContext context, ThreadService threadService,
            ForumService forumService, IConfiguration configuration, IMemoryCache cache,
            OnlineUserCache onlineUserCache)
        {
            _context = context;
            _forumService = forumService;
            _threadService = threadService;
            _configuration = configuration;
            _authCache = cache;
            _onlineUserCache = onlineUserCache;
        }

        [Route("statistics")]
        [HttpGet]
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
                    OnlineMembers = _onlineUserCache.CountUser(),
                    OnlineAnonymous = _onlineUserCache.CountAnon()
                }
            );
        }

        [Route("active-threads")]
        [HttpGet]
        public JsonResult ActiveThreads()
        {
            var query = _threadService.List(q => q.Take(8));
            return new JsonResult(query);
        }

        [Route("active-forums")]
        [HttpGet]
        public JsonResult ActiveForums([FromQuery(Name = "categories[]")] List<string> categories)
        {
            // get 8 of each categories
            var query = _forumService.List(q => q
                .Where(x => categories.Contains(x.Category.Slug))
                .GroupBy(c => c.CategoryId)
                .SelectMany(g => g.OrderByDescending(x => x.Priority).Take(5))
            );
            return new JsonResult(query);
        }


        // Handle tracking user by open logging api. user
        // For a forum application, real time is not a critical requirement (~10 mins delay is okay)
        // We will migrate to SignalR later on (if we have enough time)
        [Route("logs")]
        [HttpPost]
        public ActionResult Track()
        {
            var cookies = Request.Cookies[_configuration["JwtConfig:Refresh:Cookies"]];
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            if (string.IsNullOrWhiteSpace(cookies) || !_authCache.TryGetValue(cookies, out JwtToken value))
            {
                _onlineUserCache.PutAnon(ip);
                return new OkResult();
            }

            _onlineUserCache.PutUser(ip, value.User.Username);
            return new OkResult();
        }

        // Untrack the user
        [Route("logs")]
        [HttpDelete]
        public ActionResult UnTrack()
        {
            var cookies = Request.Cookies[_configuration["JwtConfig:Refresh:Cookies"]];
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            if (string.IsNullOrWhiteSpace(cookies) || !_authCache.TryGetValue(cookies, out JwtToken value))
            {
                _onlineUserCache.Remove(ip);
                return new OkResult();
            }

            _onlineUserCache.Remove(value.User.Username);
            return new OkResult();
        }
    }

    public class OnlineUserCache
    {
        public const int TrackPeriod = 300;
        private IDictionary<string, DateTime> _onlineAnons = new Dictionary<string, DateTime>();
        private IDictionary<string, DateTime> _onlineMembers = new Dictionary<string, DateTime>();

        public void PutAnon(string ip)
        {
            PutUser(ip, string.Empty);
        }

        public void PutUser(string ip, string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                _onlineAnons[ip] = DateTime.Now;
                RemoveInactiveEntry();
                return;
            }

            _onlineMembers[username] = DateTime.Now;
            _onlineAnons.Remove(ip);
            RemoveInactiveEntry();
        }

        public void Remove(string entry)
        {
            if (string.IsNullOrEmpty(entry)) return;

            _onlineAnons.Remove(entry);
            _onlineMembers.Remove(entry);
        }

        private void RemoveInactiveEntry()
        {
            var expireTime = DateTime.Now.AddSeconds(-100 - TrackPeriod);
            _onlineMembers = _onlineMembers.Where(kv => kv.Value > expireTime)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            _onlineAnons = _onlineAnons.Where(kv => kv.Value > expireTime)
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public int CountUser()
        {
            return _onlineMembers.Count;
        }

        public int CountAnon()
        {
            return _onlineAnons.Count;
        }
    }
}