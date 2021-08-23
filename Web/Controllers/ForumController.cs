using System.Linq;
using Core.Services;
using DAL.Models.Forum;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace Web.Controllers
{
    [ApiController]
    [Route("/api/forums")]
    public class ForumController : AbstractQueryableCrudController<Forum>
    {
        public ForumController(ForumService service) : base(service)
        {
        }

        protected override IQueryable<Forum> Query(IQueryable<Forum> query)
        {
            var search = GetQueryString("search");
            var category = GetQueryString("category");

            query = query.Where(x => x.Title.Contains(search));
            if (string.IsNullOrWhiteSpace(category) || category.Equals("_all")) return query;
            return query.Where(x => x.Category.Slug.Equals(category));
        }
    }
}