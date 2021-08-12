using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("/api/home")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public JsonResult Index()
        {
            return new JsonResult(new { HelloWorld = true });
        }
    }
}