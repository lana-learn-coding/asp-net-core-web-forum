using System.Linq;
using Core.Services;
using DAL.Models.Topic;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Public.Topic
{
    [ApiController]
    [Route("/api/tags")]
    public class TagController : AbstractQueryableCrudController<Tag>
    {
        public TagController(TagService tagService) : base(tagService)
        {
        }

        protected override IQueryable<Tag> Query(IQueryable<Tag> query)
        {
            var isSpeciality = GetQueryString("speciality");
            var search = GetQueryString("search");
            if (bool.TryParse(isSpeciality, out var speciality) && speciality)
                query = query.Where(x => x.Specialties.Count > 0);
            return query.Where(x => x.Name.Contains(search));
        }
    }
}