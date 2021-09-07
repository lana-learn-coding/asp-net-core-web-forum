using System.Linq;
using Core.Model;
using Core.Services;
using DAL.Models;
using DAL.Models.Forum;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Public
{
    [ApiController]
    [Route("/api/forums")]
    public class ForumController : AbstractQueryableCrudController<Forum, ForumView>
    {
        public ForumController(ForumService service) : base(service)
        {
        }

        protected override IQueryable<Forum> Query(IQueryable<Forum> query)
        {
            var search = GetQueryString("search");
            var category = GetQueryString("category");
            var language = GetQueryString("language");

            if (!HttpContext.User.IsInRole("Admin"))
                query = query.Where(x => (short)x.ForumAccess < (short)AccessMode.Internal);

            if (!string.IsNullOrWhiteSpace(category) && !category.Equals("_all"))
                query = query.Where(x => x.Category.Slug.Equals(category));
            if (!string.IsNullOrWhiteSpace(language))
                query = query.Where(x => x.Language.Slug.Equals(language));

            return query.Where(x => x.Title.Contains(search));
        }
    }
}