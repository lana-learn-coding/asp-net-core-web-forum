using System.Linq;
using Core.Model;
using Core.Services;
using DAL.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Admin
{
    [ApiController]
    [Route("/api/admin/users")]
    public class AdminUserController : AbstractAdminController<User, UserView>
    {
        public AdminUserController(UserService service) : base(service)
        {
        }

        protected override IQueryable<User> Query(IQueryable<User> query)
        {
            var search = GetQueryString("search");
            return query.Where(x => x.Username.Contains(search) || x.Email.Contains(search));
        }
    }
}