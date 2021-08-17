using System.Linq;
using Core.Services;
using DAL.Models.Topic;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace Web.Controllers
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
            var search = GetQueryString("search");
            return query.Where(x => x.Name.Contains(search));
        }
    }
}