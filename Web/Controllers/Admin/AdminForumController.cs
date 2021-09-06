using System.Linq;
using Core.Model;
using Core.Services;
using DAL.Models.Forum;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Admin
{
    [ApiController]
    [Route("/api/admin/forums")]
    public class AdminForumController : AbstractAdminController<Forum, ForumView>
    {
        public AdminForumController(ForumService service) : base(service)
        {
        }

        protected override IQueryable<Forum> Query(IQueryable<Forum> query)
        {
            var search = GetQueryString("search");
            var category = GetQueryString("category");
            var forumAccess = GetQueryString("forumAccess");
            var threadAccess = GetQueryString("threadAccess");

            if (int.TryParse(forumAccess, out var fAccess))
                query = query.Where(x => (int)x.ForumAccess == fAccess);
            if (int.TryParse(threadAccess, out var tAccess))
                query = query.Where(x => (int)x.ThreadAccess == tAccess);

            query = query.Where(x => x.Title.Contains(search));
            if (string.IsNullOrWhiteSpace(category) || category.Equals("_all")) return query;
            return query.Where(x => x.Category.Slug.Equals(category));
        }
    }
}