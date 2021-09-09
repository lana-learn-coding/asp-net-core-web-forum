using System.Linq;
using Core.Dto;
using Core.Services;
using DAL.Models.Forum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Public
{
    [ApiController]
    [Route("/api/posts")]
    public class PostController
    {
        private readonly PostService _service;

        public PostController(PostService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("all")]
        public virtual IActionResult All([FromQuery] string search, [FromQuery] int skip = 0,
            [FromQuery] int take = 1000)
        {
            search ??= string.Empty;
            return new JsonResult(
                _service.List(query => query
                    .Where(x => x.Content.Contains(search ?? string.Empty))
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                )
            );
        }

        [HttpGet]
        [Route("")]
        public virtual IActionResult Index([FromQuery] PageQuery pageQuery, [FromQuery] string search,
            [FromQuery] string thread, [FromQuery] string user)
        {
            search ??= string.Empty;
            return new JsonResult(_service.Page(pageQuery, query =>
            {
                if (!string.IsNullOrWhiteSpace(thread)) query = query.Where(x => x.Thread.Slug.Equals(thread));
                if (!string.IsNullOrWhiteSpace(user)) query = query.Where(x => x.User.Slug.Equals(user));
                return query.Where(x => x.Content.Contains(search));
            }));
        }

        [HttpGet]
        [Route("{slug}")]
        public virtual IActionResult Show(string slug)
        {
            return new JsonResult(_service.Get(slug));
        }

        [HttpPost]
        [Route("")]
        public virtual IActionResult Store([FromBody] Post entity)
        {
            return new JsonResult(_service.Create(entity));
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{slug}")]
        public virtual IActionResult Update(string slug, [FromBody] Post entity)
        {
            return new JsonResult(_service.Update(slug, entity));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{slug}")]
        public virtual IActionResult Destroy(string slug)
        {
            _service.Delete(slug);
            return new OkResult();
        }

        [HttpPost]
        [Route("{slug}/votes/{vote:int}")]
        [Authorize]
        public IActionResult Store(string slug, int vote)
        {
            _service.Vote(slug, (short)vote);
            return new OkResult();
        }
    }
}