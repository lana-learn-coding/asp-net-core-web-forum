using Core.Dto;
using Core.Services.Base;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Admin
{
    public abstract class AbstractAdminController<T> : AbstractQueryableCrudController<T> where T : Entity
    {
        protected AbstractAdminController(CrudService<T> service) : base(service)
        {
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("all")]
        public override IActionResult All([FromQuery] int skip = 0, [FromQuery] int take = 1000)
        {
            return base.All(skip, take);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("")]
        public override IActionResult Index([FromQuery] PageQuery pageQuery)
        {
            return base.Index(pageQuery);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("{slug}")]
        public override IActionResult Show(string slug)
        {
            return base.Show(slug);
        }
    }
}