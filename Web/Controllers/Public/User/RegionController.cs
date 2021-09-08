using System;
using System.Linq;
using Core.Services;
using DAL.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Public.User
{
    [ApiController]
    [Route("/api/regions/cities")]
    public class CityController : AbstractQueryableCrudController<City>
    {
        public CityController(CityService cityService) : base(cityService)
        {
        }

        protected override IQueryable<City> Query(IQueryable<City> query)
        {
            var search = GetQueryString("search");
            var countryId = GetQueryString("countryId");
            if (!string.IsNullOrWhiteSpace(countryId) && Guid.TryParse(countryId, out var id))
                query = query.Where(x => x.CountryId.Equals(id));
            return query.Where(x => x.Name.Contains(search) || x.Country.Name.Contains(search));
        }
    }

    [ApiController]
    [Route("/api/regions/countries")]
    public class CountryController : AbstractQueryableCrudController<Country>
    {
        public CountryController(CountryService service) : base(service)
        {
        }

        protected override IQueryable<Country> Query(IQueryable<Country> query)
        {
            var search = GetQueryString("search");
            return query.Where(x => x.Name.Contains(search));
        }
    }
}