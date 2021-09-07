using System.Linq;
using Core.Model;
using Core.Services;
using DAL;
using DAL.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Admin
{
    [ApiController]
    [Route("/api/admin/users")]
    public class AdminUserController : AbstractAdminController<User, UserView>
    {
        private readonly ModelContext _context;

        public AdminUserController(UserService service, ModelContext context) : base(service)
        {
            _context = context;
        }

        protected override IQueryable<User> Query(IQueryable<User> query)
        {
            var search = GetQueryString("search");
            return query.Where(x => x.Username.Contains(search) || x.Email.Contains(search));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("roles/all")]
        public IActionResult Roles([FromQuery] string search, [FromQuery] int skip = 0,
            [FromQuery] int take = 1000)
        {
            search ??= "";
            return new JsonResult(_context.Roles
                .Where(x => x.Name.Contains(search))
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(take)
            );
        }
    }
}