using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Core.Services;
using Core.Services.Base;
using DAL.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Web.Dto.Auth;

namespace Web.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController : Controller
    {
        private readonly UserService _service;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        // Keep refresh token in cache instead of database
        // The token will disappear after x days, or after server restart
        private readonly IMemoryCache _cache;


        public AuthController(IMapper mapper, UserService service, IConfiguration configuration, IMemoryCache cache)
        {
            _service = service;
            _configuration = configuration;
            _cache = cache;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("sign-up")]
        public IActionResult SignUp([FromBody] User entity)
        {
            return new JsonResult(_service.Create(entity));
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            var cookies = Request.Cookies[_configuration["JwtConfig:Refresh:Cookies"]];
            if (string.IsNullOrWhiteSpace(cookies))
            {
                return new OkResult();
            }

            _cache.Remove(cookies);
            RemoveAuthCookies();
            return new OkResult();
        }


        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] AuthPayload authPayload)
        {
            try
            {
                var user = _service.Verify(authPayload.Username, authPayload.Password);
                var token = GenerateToken(user, authPayload.RememberMe);
                if (!authPayload.RememberMe) return new JsonResult(token);

                // set refresh token cookies if remember me = true
                var options = new CookieOptions
                {
                    Secure = _configuration.GetValue<bool>("Https"),
                    HttpOnly = true,
                    Expires = token.RefreshExpires,
                    SameSite = SameSiteMode.Strict
                };
                Response.Cookies.Append(_configuration["JwtConfig:Refresh:Cookies"], token.RefreshToken, options);
                return new JsonResult(token);
            }
            catch (UnauthorizedException)
            {
                throw new InvalidDataException
                {
                    ["Username"] = new[] { "Wrong user name or password" },
                    ["Password"] = new[] { "Wrong user name or password" }
                };
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public IActionResult Refresh()
        {
            var cookies = Request.Cookies[_configuration["JwtConfig:Refresh:Cookies"]];
            if (string.IsNullOrWhiteSpace(cookies))
            {
                RemoveAuthCookies();
                return new UnauthorizedResult();
            }

            // refresh token expired
            if (!_cache.TryGetValue(cookies, out JwtToken value))
            {
                RemoveAuthCookies();
                return new UnauthorizedResult();
            }

            // return old token if not expired within next 6 hour
            if (value.Expires < DateTime.Now.AddHours(-6)) return new JsonResult(value);

            // generate new ones if old token expired
            var user = _service.Get(value.User.Slug);
            if (user != null) return new JsonResult(GenerateToken(user));

            // user may deleted?
            RemoveAuthCookies();
            return new UnauthorizedResult();
        }

        private void RemoveAuthCookies()
        {
            var options = new CookieOptions
            {
                Secure = _configuration.GetValue<bool>("Https"),
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddYears(-1),
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append(_configuration["JwtConfig:Refresh:Cookies"], Guid.NewGuid().ToString(), options);
        }

        private JwtToken GenerateToken(User user, bool remember = false)
        {
            var claims = new List<Claim>
            {
                new("username", user.Username),
                new("fullname", user.FullName),
                new("email", user.Email)
            };

            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

            var token = JwtHelper.GetJwtToken(
                user.Username,
                _configuration["JwtConfig:SigningKey"],
                _configuration["JwtConfig:Issuer"] ?? HttpContext.Request.Host.Host,
                _configuration["JwtConfig:Audience"] ?? HttpContext.Request.Host.Host,
                TimeSpan.FromSeconds(double.Parse(_configuration["JwtConfig:MaxAge"])),
                claims.ToArray()
            );

            var jwt = new JwtToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = token.ValidTo,
                User = _mapper.Map<AuthUser>(user)
            };
            if (!remember) return jwt;

            jwt.RefreshToken = Guid.NewGuid().ToString();
            jwt.RefreshExpires =
                DateTime.Now.Add(TimeSpan.FromSeconds(double.Parse(_configuration["JwtConfig:Refresh:MaxAge"])));
            _cache.Set(jwt.RefreshToken, jwt, jwt.RefreshExpires);
            return jwt;
        }
    }

    public static class JwtHelper
    {
        public static JwtSecurityToken GetJwtToken(
            string username,
            string uniqueKey,
            string issuer,
            string audience,
            TimeSpan expiration,
            IEnumerable<Claim> additionalClaims = null)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                // this guarantees the token is unique
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (additionalClaims != null)
            {
                var claimList = new List<Claim>(claims);
                claimList.AddRange(additionalClaims);
                claims = claimList.ToArray();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(uniqueKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer,
                audience,
                expires: DateTime.UtcNow.Add(expiration),
                claims: claims,
                signingCredentials: credentials
            );
        }
    }
}