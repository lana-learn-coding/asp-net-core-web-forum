using System.Linq;
using Core.Services;
using DAL.Models.Topic;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Public.Topic
{
    [ApiController]
    [Route("/api/languages")]
    public class LanguageController : AbstractQueryableCrudController<Language>
    {
        public LanguageController(LanguageService languageService) : base(languageService)
        {
        }

        protected override IQueryable<Language> Query(IQueryable<Language> query)
        {
            var search = GetQueryString("search");
            return query.Where(x => x.Name.Contains(search));
        }
    }
}