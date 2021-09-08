using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Model;
using Core.Services.Base;
using DAL.Models.Auth;

namespace Core.Services
{
    public class UserInfoService : CrudService<UserInfo, UserInfoView>
    {
        private readonly IConfigurationProvider _mapperConfig;

        public UserInfoService(DbContext context, IConfigurationProvider mapperConfig) : base(context)
        {
            DefaultSort = new List<string> { "UpdatedAt desc" };
            _mapperConfig = mapperConfig;
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

            entity.User = current.User;

            base.Update(current, entity);
        }

        protected override IQueryable<UserInfoView> Query(IQueryable<UserInfo> queryable)
        {
            return queryable.Include("User")
                .Include("WorkSpecialities")
                .ProjectTo<UserInfoView>(_mapperConfig);
        }
    }
}