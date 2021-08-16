using System;
using System.Linq;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UserController
    {
        private readonly ModelContext _context;

        public UserController(ModelContext ctx)
        {
            _context = ctx;
        }

        [HttpGet]
        [Route("")]
        public JsonResult Index()
        {
            return new JsonResult(_context.Users.ToList());
        }
    }
}