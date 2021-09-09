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
        public virtual IActionResult Index([FromQuery] PageQuery pageQuery, [FromQuery] string search,
            [FromQuery] string speciality, [FromQuery] string city, [FromQuery] string country,
            [FromQuery] string position)
        {
            search ??= "";
            return new JsonResult(_service.Page(pageQuery, q =>
            {
                if (!string.IsNullOrWhiteSpace(search))
                    q = q.Where(x => x.User.Email.Contains(search) && x.ShowEmail || x.User.Username.Contains(search));
                if (!string.IsNullOrWhiteSpace(speciality))
                    q = q.Where(x => x.WorkSpecialities.Any(s => s.Slug.Equals(speciality)));
                if (!string.IsNullOrWhiteSpace(city))
                    q = q.Where(x => x.WorkCity != null && x.WorkCity.Slug.Equals(city) && x.ShowWorkAddress);
                if (!string.IsNullOrWhiteSpace(country))
                    q = q.Where(x => x.WorkCountry != null && x.WorkCountry.Slug.Equals(country) && x.ShowWorkAddress);
                if (!string.IsNullOrWhiteSpace(position))
                    q = q.Where(x =>
                        x.WorkPosition != null && x.WorkPosition.Slug.Equals(position) && x.ShowWorkExperience);
                return q;
            }));
        }

        [HttpGet]
        [Route("{slug}")]
        public virtual IActionResult Show(string slug)
        {
            return new JsonResult(_service.Get(slug));
        }
    }
}