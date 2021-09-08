using System;
using System.Linq;
using AutoMapper;
using Core.Dto;
using Core.Model.Write;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Public
{
    [ApiController]
    [Route("/api/threads")]
    public class ThreadController
    {
        private readonly IMapper _mapper;
        private readonly ThreadService _service;

        public ThreadController(ThreadService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public virtual IActionResult Index([FromQuery] PageQuery pageQuery, [FromQuery] string forum,
            [FromQuery] string search, [FromQuery] string user)
        {
            search ??= "";
            return new JsonResult(_service.Page(pageQuery, q =>
                {
                    if (!string.IsNullOrWhiteSpace(forum)) q = q.Where(x => x.Forum.Slug.Equals(forum));
                    if (!string.IsNullOrWhiteSpace(user))
                        q = q.Where(x => x.Posts.Any(p => p.Id.Equals(x.Id) && p.User.Slug.Equals(user)));
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

        [HttpPost]
        [Authorize]
        public IActionResult Store([FromBody] CreateThreadUser entity)
        {
            return new JsonResult(_service.Create(_mapper.Map<CreateThreadAdmin>(entity)));
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