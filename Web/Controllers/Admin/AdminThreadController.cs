using System;
using System.Linq;
using Core.Dto;
using Core.Model.Write;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Admin
{
    [ApiController]
    [Route("/api/admin/threads")]
    [Authorize(Roles = "Admin")]
    public class AdminThreadController
    {
        private readonly ThreadService _service;

        public AdminThreadController(ThreadService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("")]
        public virtual IActionResult Index([FromQuery] PageQuery pageQuery, [FromQuery] string forum,
            [FromQuery] string search, [FromQuery] int? status)
        {
            search ??= "";
            return new JsonResult(_service.Page(pageQuery, q =>
                {
                    if (!string.IsNullOrWhiteSpace(forum)) q = q.Where(x => x.Forum.Slug.Equals(forum));
                    if (status != null) q = q.Where(x => (int)x.Status == status);
                    return q.Where(x => x.Title.Contains(search) || x.Tags.Any(t => t.Name.Contains(search)));
                }
            ));
        }

        [HttpGet]
        [Route("all")]
        public virtual IActionResult Index([FromQuery] PageQuery pageQuery, [FromQuery] string search)
        {
            search ??= "";
            return new JsonResult(_service.Query().Where(x => x.Title.Contains(search)));
        }

        [HttpGet]
        [Route("{slug}")]
        public virtual IActionResult Show(string slug)
        {
            return Guid.TryParse(slug, out var id)
                ? new JsonResult(id)
                : new JsonResult(_service.Get(slug));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Store([FromBody] CreateThreadAdmin entity)
        {
            return new JsonResult(_service.Create(entity));
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{slug}")]
        public IActionResult Update(string slug, [FromBody] CreateThreadAdmin entity)
        {
            return new JsonResult(_service.Update(slug, entity));
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