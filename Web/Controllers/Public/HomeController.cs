using Core.Services;

namespace Web.Controllers.Public
{
    public class HomeController
    {
        private readonly ForumService _forumService;
        private readonly CategoryService _categoryService;

        public HomeController(ForumService forumService, CategoryService categoryService)
        {
            _forumService = forumService;
            _categoryService = categoryService;
        }
    }
}