using System.Linq;
using Core.Model;
using Core.Services;
using DAL.Models.Forum;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Public
{
    [ApiController]
    [Route("/api/posts")]
    public class PostController : AbstractQueryableCrudController<Post, PostView>
    {
        public PostController(PostService service) : base(service)
        {
        }

        protected override IQueryable<Post> Query(IQueryable<Post> query)
        {
            var search = GetQueryString("search");
            var thread = GetQueryString("thread");

            if (!string.IsNullOrWhiteSpace(thread)) query = query.Where(x => x.Thread.Slug.Equals(thread));
            return query.Where(x => x.Content.Contains(search));
        }

        [HttpPost]
        public override IActionResult Store([FromBody] Post entity)
        {
            return base.Store(entity);
        }
    }
}