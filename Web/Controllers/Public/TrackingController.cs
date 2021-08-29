using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL;
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

        public TrackingController(ModelContext context)
        {
            _context = context;
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
            var forums = _context.Forums
                .Where(x => categories.Contains(x.Category.Slug))
                .GroupBy(c => c.CategoryId)
                .SelectMany(g => g.OrderByDescending(x => x.Priority).Take(5))
                .Include("Category");
            return new JsonResult(forums);
        }

        [Route("logs")]
        [HttpPost]
        public ActionResult Track()
        {
            return new OkResult();
        }
    }
}