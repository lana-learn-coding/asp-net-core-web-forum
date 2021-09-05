using System;
using System.Linq;
using Core.Dto;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Public
{
    [ApiController]
    [Route("/api/threads")]
    public class ThreadController
    {
        private readonly ThreadService _service;

        public ThreadController(ThreadService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public virtual IActionResult Index([FromQuery] PageQuery pageQuery, [FromQuery] string forum,
            [FromQuery] string search)
        {
            search ??= "";
            return new JsonResult(_service.Page(pageQuery, q =>
                {
                    if (!string.IsNullOrWhiteSpace(forum)) q = q.Where(x => x.Forum.Slug.Equals(forum));
                    return q.Where(x => x.Title.Contains(search) || x.Tags.Any(t => t.Name.Contains(search)));
                }
            ));
        }


        [HttpGet]
        [Route("{slug}")]
        public virtual IActionResult Show(string slug)
        {
            return Guid.TryParse(slug, out var id)
                ? new JsonResult(id)
                : new JsonResult(_service.Get(slug));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{slug}")]
        public IActionResult Destroy(string slug)
        {
            _service.Delete(slug);
            return new OkResult();
        }
    }
}