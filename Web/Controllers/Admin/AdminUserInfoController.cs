using System.Linq;
using Core.Model;
using Core.Services;
using DAL.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Admin
{
    [ApiController]
    [Route("/api/admin/users/infos")]
    public class AdminUserInfoController : AbstractAdminController<UserInfo, UserInfoView>
    {
        public AdminUserInfoController(UserInfoService service) : base(service)
        {
        }

        protected override IQueryable<UserInfo> Query(IQueryable<UserInfo> query)
        {
            var search = GetQueryString("search");
            return query.Where(x => x.User.Username.Contains(search) || x.User.Email.Contains(search));
        }
    }
}