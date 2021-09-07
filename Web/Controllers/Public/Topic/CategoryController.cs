using System.Linq;
using Core.Services;
using DAL.Models.Topic;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Public.Topic
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoryController : AbstractQueryableCrudController<Category>
    {
        public CategoryController(CategoryService categoryService) : base(categoryService)
        {
        }

        protected override IQueryable<Category> Query(IQueryable<Category> query)
        {
            var search = GetQueryString("search");
            return query.Where(x => x.Name.Contains(search));
        }
    }
}