using Core.Services.Base;
using DAL.Models;

namespace Web.Controllers
{
    public abstract class AbstractQueryableCrudController<T> : AbstractCrudController<T> where T : Entity
    {
        protected AbstractQueryableCrudController(CrudService<T, T> service) : base(service)
        {
        }

        // Help get query from param. then we can filter based on that query
        protected string GetQueryString(string name)
        {
            if (HttpContext == null) return "";
            var stringValues = HttpContext.Request.Query[name];
            return stringValues.Count == 0 ? "" : stringValues[0];
        }
    }
}