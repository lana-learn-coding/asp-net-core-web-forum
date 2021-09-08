using System.Linq;
using Core.Services;
using DAL.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Public.User
{
    [ApiController]
    [Route("/api/exp/experiences")]
    public class ExperienceController : AbstractQueryableCrudController<Experience>
    {
        public ExperienceController(ExperienceService service) : base(service)
        {
        }

        protected override IQueryable<Experience> Query(IQueryable<Experience> query)
        {
            var search = GetQueryString("search");
            return query.Where(x => x.Measurement.Contains(search));
        }
    }

    [ApiController]
    [Route("/api/exp/positions")]
    public class PositionController : AbstractQueryableCrudController<Position>
    {
        public PositionController(PositionService service) : base(service)
        {
        }

        protected override IQueryable<Position> Query(IQueryable<Position> query)
        {
            var search = GetQueryString("search");
            return query.Where(x => x.Name.Contains(search));
        }
    }
}