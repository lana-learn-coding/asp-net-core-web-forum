using System.Linq;
using Core.Dto;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Public.User
{
    [ApiController]
    [Route("/api/users/infos")]
    public class UserInfoController
    {
        private readonly UserInfoService _service;

        public UserInfoController(UserInfoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public virtual IActionResult Index([FromQuery] PageQuery pageQuery, [FromQuery] string search)
        {
            var page = _service.Page(pageQuery,
                q => q.Where(x => x.User.Username.Contains(search) || x.User.Email.Contains(search)));
            return new JsonResult(page);
        }

        [HttpGet]
        [Route("{slug}")]
        public virtual IActionResult Show(string slug)
        {
            return new JsonResult(_service.Get(slug));
        }
    }
}