using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Helper;
using Core.Model;
using Core.Services.Base;
using DAL.Models.Auth;
using DAL.Models.Topic;
using Microsoft.AspNetCore.Http;

namespace Core.Services
{
    public class UserInfoService : CrudService<UserInfo, UserInfoView>
    {
        private readonly HttpContext _httpContext;
        private readonly IConfigurationProvider _mapperConfig;

        public UserInfoService(DbContext context, IConfigurationProvider mapperConfig,
            IHttpContextAccessor httpContextAccessor) : base(context)
        {
            DefaultSort = new List<string> { "UpdatedAt desc" };
            _mapperConfig = mapperConfig;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public override UserInfoView Create(UserInfo entity)
        {
            throw new ConflictException("Not supported! User info will automatically created along with account");
        }

        protected override void Delete(UserInfo entity)
        {
            throw new ConflictException("Please delete user account instead");
        }

        protected override void Update(UserInfo current, UserInfo entity)
        {
            if (entity.WorkCityId != null)
            {
                var city = Context.Set<City>().Include("Country")
                               .FirstOrDefault(x => x.Id.Equals(entity.WorkCityId.Value))
                           ?? throw new InvalidDataException("WorkCityId", "city not found");
                entity.WorkCountryId = city.CountryId;
            }

            Context.Entry(current).Reference(x => x.User).Load();
            entity.User = current.User;
            current.WorkSpecialitiesIds = entity.WorkSpecialitiesIds;
            FillSpecialities(current);
            base.Update(current, entity);
        }

        public UserInfoView Me()
        {
            var id = _httpContext.User.Id() ?? throw new UnauthorizedException();
            var info = DbSet.Where(x => x.Id.Equals(id))
                .Include("User")
                .Include("WorkSpecialities")
                .ProjectTo<UserInfoView>(_mapperConfig, new { showUserInfo = true })
                .FirstOrDefault();
            return info ?? throw new DataNotFoundException("User not found");
        }

        public UserInfoView MeUpdate(UserInfo entity)
        {
            var id = _httpContext.User.Id() ?? throw new UnauthorizedException();
            var info = DbSet.Where(x => x.Id.Equals(id))
                .Include("User")
                .Include("WorkSpecialities")
                .FirstOrDefault();
            Update(info, entity);
            return Me();
        }

        private void FillSpecialities(UserInfo info)
        {
            var specialitiesIds = info.WorkSpecialitiesIds;
            if (specialitiesIds == null || specialitiesIds.Count == 0)
            {
                info.WorkSpecialities.Clear();
                return;
            }

            var specialities = Context.Set<Specialty>().Where(x => specialitiesIds.Contains(x.Id));
            info.WorkSpecialities.Clear();
            foreach (var speciality in specialities) info.WorkSpecialities.Add(speciality);
        }

        protected override IQueryable<UserInfoView> Query(IQueryable<UserInfo> queryable)
        {
            var isAdmin = _httpContext.User.IsAdmin();
            return queryable.Include("User")
                .Include("WorkSpecialities")
                .ProjectTo<UserInfoView>(_mapperConfig, new { showUserInfo = isAdmin });
        }
    }
}