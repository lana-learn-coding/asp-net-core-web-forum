using Core.Helper;
using Core.Services;
using Core.Services.Base;
using DAL.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Dto.User;

namespace Web.Controllers.Public.User
{
    [ApiController]
    [Route("/api/me")]
    [Authorize]
    public class MeController : Controller
    {
        private readonly UserInfoService _info;
        private readonly UserService _service;

        public MeController(UserService service, UserInfoService userInfoService)
        {
            _service = service;
            _info = userInfoService;
        }

        [HttpGet]
        [Route("")]
        public virtual IActionResult Me()
        {
            return new JsonResult(_info.Me());
        }

        [HttpPost]
        [Route("profile")]
        public virtual IActionResult MeUpdate([FromBody] UserInfo info)
        {
            return new JsonResult(_info.MeUpdate(info));
        }

        [HttpPost]
        [Route("change-password")]
        public virtual IActionResult ChangePassword([FromBody] ChangePassword request)
        {
            try
            {
                var username = User.GetClaim("username") ?? throw new UnauthorizedException();
                var user = _service.Verify(username, request.Password);
                if (request.Password.Equals(request.NewPassword))
                    throw new InvalidDataException("NewPassword", "New password must different from old one");
                _service.ChangePassword(user.Slug, request.NewPassword);
                return new OkResult();
            }
            catch (UnauthorizedException)
            {
                throw new InvalidDataException("Password", "Wrong password");
            }
        }

        [HttpPost]
        [Route("change-avatar")]
        public virtual IActionResult ChangeAvatar([FromBody] ChangeAvatar request)
        {
            var userId = User.Id() ?? throw new UnauthorizedException();
            _service.ChangeAvatar(userId.ToString(), request.Avatar);
            return new OkResult();
        }
    }
}