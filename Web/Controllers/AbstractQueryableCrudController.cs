using Core.Services.Base;
using DAL.Models;

namespace Web.Controllers
{
    public abstract class AbstractQueryableCrudController<W, R> : AbstractCrudController<W, R>
        where W : Entity where R : IIdentified
    {
        protected AbstractQueryableCrudController(CrudService<W, R> service) : base(service)
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

    public abstract class AbstractQueryableCrudController<T> : AbstractQueryableCrudController<T, T> where T : Entity
    {
        protected AbstractQueryableCrudController(CrudService<T, T> service) : base(service)
        {
        }
    }
}