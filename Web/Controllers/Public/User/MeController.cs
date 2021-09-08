﻿using AutoMapper;
using Core.Helper;
using Core.Services;
using Core.Services.Base;
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
        private readonly IMapper _mapper;
        private readonly UserService _service;

        public MeController(UserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        private string Id => User.Id()?.ToString() ?? throw new UnauthorizedException();

        [HttpGet]
        [Route("")]
        public virtual IActionResult Me()
        {
            return new JsonResult(_mapper.Map<MeUser>(_service.Get(Id)));
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