using System;
using System.Linq;
using Core.Dto;
using Core.Services.Base;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public abstract class AbstractCrudController<T> : Controller where T : Entity
    {
        protected readonly CrudService<T, T> Service;

        protected AbstractCrudController(CrudService<T, T> service)
        {
            Service = service;
        }

        [HttpGet]
        [Route("all")]
        public virtual IActionResult All([FromQuery] int skip = 0, [FromQuery] int take = 1000)
        {
            return new JsonResult(
                Service.List(query => Query(query)
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                )
            );
        }

        [HttpGet]
        [Route("")]
        public virtual IActionResult Index([FromQuery] PageQuery pageQuery)
        {
            return new JsonResult(Service.Page(pageQuery, Query));
        }

        protected abstract IQueryable<T> Query(IQueryable<T> query);

        [HttpGet]
        [Route("{slug}")]
        public virtual IActionResult Show(string slug)
        {
            return Guid.TryParse(slug, out var id)
                ? new JsonResult(id)
                : new JsonResult(Service.Get(slug));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Store([FromBody] T entity)
        {
            return new JsonResult(Service.Create(entity));
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{slug}")]
        public IActionResult Update(string slug, [FromBody] T entity)
        {
            return new JsonResult(Service.Update(slug, entity));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{slug}")]
        public IActionResult Destroy(string slug)
        {
            Service.Delete(slug);
            return new OkResult();
        }
    }

    public class SimpleCrudController<T> : AbstractCrudController<T> where T : Entity
    {
        public SimpleCrudController(CrudService<T, T> service) : base(service)
        {
        }

        // No filter
        protected override IQueryable<T> Query(IQueryable<T> query)
        {
            return query;
        }
    }
}