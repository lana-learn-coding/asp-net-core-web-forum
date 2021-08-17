using System.Linq;
using Core.Services;
using DAL.Models.Topic;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace Web.Controllers
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoryController : SimpleCrudController<Category>
    {
        public CategoryController(CategoryService categoryService) : base(categoryService)
        {
        }
    }
}