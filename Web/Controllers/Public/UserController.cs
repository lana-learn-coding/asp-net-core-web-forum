using System.Linq;
using AutoMapper;
using Core.Model;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Dto.User;

namespace Web.Controllers.Public
{
    [ApiController]
    [Route("/api/users")]
    public class UserController
    {
        private readonly IMapper _mapper;
        private readonly UserService _service;

        public UserController(UserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("all")]
        public virtual IActionResult All([FromQuery] string search, [FromQuery] int skip = 0,
            [FromQuery] int take = 1000)
        {
            search ??= string.Empty;
            return new JsonResult(
                _service.Query()
                    .Where(x => x.Username.Contains(search) || x.Email.Contains("search"))
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                    .AsEnumerable<UserView>()
                    .Select(x => _mapper.Map<PublicUser>(x))
            );
        }

        [HttpGet]
        [Route("{slug}")]
        public virtual IActionResult Show(string slug)
        {
            return new JsonResult(_mapper.Map<PublicUser>(_service.Get(slug)));
        }
    }
}