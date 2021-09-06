using System;
using System.Linq;
using Core.Dto;
using Core.Services;
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
        [Route("all")]
        public virtual IActionResult Index([FromQuery] string search, [FromQuery] int skip = 0,
            [FromQuery] int take = 1000)
        {
            search ??= "";
            return new JsonResult(_service.List(q => q
                .Where(x => x.Title.Contains(search) || x.Tags.Any(t => t.Name.Contains(search)))
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(take)
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
    }
}