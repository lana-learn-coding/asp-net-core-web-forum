using System.Linq;
using Core.Services;
using DAL.Models.Topic;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Public.Topic
{
    [ApiController]
    [Route("/api/specialties")]
    public class SpecialtyController : AbstractQueryableCrudController<Specialty>
    {
        public SpecialtyController(SpecialtyService tagService) : base(tagService)
        {
        }

        protected override IQueryable<Specialty> Query(IQueryable<Specialty> query)
        {
            var search = GetQueryString("search");
            return query.Where(x => x.Tag.Name.Contains(search));
        }
    }
}