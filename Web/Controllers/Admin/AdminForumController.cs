using System.Linq;
using Core.Services;
using DAL.Models.Forum;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Admin
{
    [ApiController]
    [Route("/api/admin/forums")]
    public class AdminForumController : AbstractAdminController<Forum>
    {
        public AdminForumController(ForumService service) : base(service)
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