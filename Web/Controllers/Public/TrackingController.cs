using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Model;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Web.Dto.Home;

namespace Web.Controllers.Public
{
    [ApiController]
    [Route("/api/tracking")]
    public class TrackingController : Controller
    {
        private const int TrackPeriod = 1000;

        private readonly ModelContext _context;

        private readonly IConfigurationProvider _mapperConfig;

        public TrackingController(ModelContext context, IConfigurationProvider mapperConfig)
        {
            _context = context;
            _mapperConfig = mapperConfig;
        }

        [Route("statistics")]
        [HttpGet]
        [ResponseCache(Duration = TrackPeriod)]
        public JsonResult Statistic()
        {
            var totalUsers = _context.Users.Count();
            return new JsonResult(new ForumStatistic
                {
                    TotalMembers = totalUsers,
                    TotalPosts = 27694,
                    TotalThreads = 5528,
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
            return new JsonResult(new List<string>());
        }

        [Route("active-forums")]
        [HttpGet]
        [ResponseCache(Duration = 300)]
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

        [Route("logs")]
        [HttpPost]
        public ActionResult Track()
        {
            return new OkResult();
        }
    }
}