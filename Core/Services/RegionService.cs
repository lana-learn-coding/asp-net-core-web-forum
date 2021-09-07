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
    }

    public class CountryService : SimpleCrudService<Country>
    {
        public CountryService(DbContext context) : base(context)
        {
            DefaultSort = new List<string> { "Name" };
        }
    }
}