using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Core.Services.Base;
using DAL.Models.Auth;

namespace Core.Services
{
    public class CityService : SimpleCrudService<City>
    {
        public CityService(DbContext context) : base(context)
        {
            DefaultSort = new List<string> { "Name" };
        }

        protected override IQueryable<City> Query(IQueryable<City> queryable)
        {
            return queryable.Include("Country");
        }

        protected override City FindForWrite(string slug, Func<IQueryable<City>, IQueryable<City>> query)
        {
            return base.FindForWrite(slug, q => q.Include("UserInfos.User"));
        }
    }

    public class CountryService : SimpleCrudService<Country>
    {
        public CountryService(DbContext context) : base(context)
        {
            DefaultSort = new List<string> { "Name" };
        }

        protected override Country FindForWrite(string slug, Func<IQueryable<Country>, IQueryable<Country>> query)
        {
            return base.FindForWrite(slug, q => q.Include("UserInfos.User"));
        }

        protected override void Delete(Country entity)
        {
            foreach (var userInfo in entity.UserInfos)
            {
                userInfo.WorkCityId = null;
                userInfo.WorkCountryId = null;
            }

            base.Delete(entity);
        }
    }
}