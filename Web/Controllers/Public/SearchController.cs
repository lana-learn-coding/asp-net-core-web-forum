using System.Linq;
using Core.Dto;
using Core.Model;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Dto.Home;

namespace Web.Controllers.Public
{
    [ApiController]
    [Route("/api/search")]
    public class SearchController
    {
        private readonly PostService _postService;
        private readonly ThreadService _threadService;

        public SearchController(PostService postService, ThreadService threadService)
        {
            _postService = postService;
            _threadService = threadService;
        }

        [HttpGet]
        [Route("")]
        public virtual IActionResult Index([FromQuery] AdvancedSearch query)
        {
            query.Type ??= string.Empty;
            return new JsonResult(query.Type.Equals("post") ? QueryPost(query) : QueryThread(query));
        }

        private Page<ThreadView> QueryThread(AdvancedSearch query)
        {
            return _threadService.Page(query, q =>
            {
                if (!string.IsNullOrWhiteSpace(query.Search))
                    q = q.Where(x => x.Title.Contains(query.Search));
                if (!string.IsNullOrWhiteSpace(query.Forum))
                    q = q.Where(x => x.Forum.Slug.Equals(query.Forum));
                if (!string.IsNullOrWhiteSpace(query.Tag))
                    q = q.Where(x => x.Tags.Any(t => t.Slug.Equals(query.Tag)));
                if (!string.IsNullOrWhiteSpace(query.Thread))
                    q = q.Where(x => x.Slug.Equals(query.Thread));
                if (!string.IsNullOrWhiteSpace(query.Speciality))
                    q = q.Where(x => x.Posts.Any(p => p.Id.Equals(x.Id) &&
                                                      p.User.UserInfo.WorkSpecialities.Any(s =>
                                                          x.Slug.Equals(query.Speciality))));
                if (!string.IsNullOrWhiteSpace(query.Experience))
                    q = q.Where(x => x.Posts.Any(p => p.Id.Equals(x.Id) &&
                                                      p.User.UserInfo.ShowWorkExperience &&
                                                      p.User.UserInfo.WorkExperience != null &&
                                                      p.User.UserInfo.WorkExperience.Slug.Equals(query.Experience)));
                if (!string.IsNullOrWhiteSpace(query.Position))
                    q = q.Where(x => x.Posts.Any(p => p.Id.Equals(x.Id) &&
                                                      p.User.UserInfo.ShowWorkExperience &&
                                                      p.User.UserInfo.WorkPosition != null &&
                                                      p.User.UserInfo.WorkPosition.Slug.Equals(query.Position)));
                if (!string.IsNullOrWhiteSpace(query.City))
                    q = q.Where(x => x.Posts.Any(p => p.Id.Equals(x.Id) &&
                                                      p.User.UserInfo.ShowWorkAddress &&
                                                      p.User.UserInfo.WorkCity != null &&
                                                      p.User.UserInfo.WorkCity.Slug.Equals(query.City)));
                if (!string.IsNullOrWhiteSpace(query.Country))
                    q = q.Where(x => x.Posts.Any(p => p.Id.Equals(x.Id) &&
                                                      p.User.UserInfo.ShowWorkAddress &&
                                                      p.User.UserInfo.WorkCountry != null &&
                                                      p.User.UserInfo.WorkCountry.Slug.Equals(query.Country)));
                if (!string.IsNullOrWhiteSpace(query.User))
                    q = q.Where(x => x.Posts.Any(p => p.Id.Equals(x.Id) &&
                                                      (p.User.UserInfo.ShowEmail && p.User.Email.Contains(query.User) ||
                                                       p.User.Username.Contains(query.User))));
                return q;
            });
        }

        private Page<PostView> QueryPost(AdvancedSearch query)
        {
            return _postService.Page(query, q =>
            {
                if (!string.IsNullOrWhiteSpace(query.Search))
                    q = q.Where(x => x.Content.Contains(query.Search));
                if (!string.IsNullOrWhiteSpace(query.Forum))
                    q = q.Where(x => x.Thread.Forum.Slug.Equals(query.Forum));
                if (!string.IsNullOrWhiteSpace(query.Tag))
                    q = q.Where(x => x.Thread.Tags.Any(t => t.Slug.Equals(query.Tag)));
                if (!string.IsNullOrWhiteSpace(query.Thread))
                    q = q.Where(x => x.Thread.Slug.Equals(query.Thread));
                if (!string.IsNullOrWhiteSpace(query.Speciality))
                    q = q.Where(x => x.User.UserInfo.WorkSpecialities.Any(s => x.Slug.Equals(query.Speciality)));
                if (!string.IsNullOrWhiteSpace(query.Experience))
                    q = q.Where(x =>
                        x.User.UserInfo.ShowWorkExperience && x.User.UserInfo.WorkExperience != null &&
                        x.User.UserInfo.WorkExperience.Slug.Equals(query.Experience));
                if (!string.IsNullOrWhiteSpace(query.Position))
                    q = q.Where(x =>
                        x.User.UserInfo.ShowWorkExperience && x.User.UserInfo.WorkPosition != null &&
                        x.User.UserInfo.WorkPosition.Slug.Equals(query.Position));
                if (!string.IsNullOrWhiteSpace(query.City))
                    q = q.Where(x =>
                        x.User.UserInfo.ShowWorkAddress && x.User.UserInfo.WorkCity != null &&
                        x.User.UserInfo.WorkCity.Slug.Equals(query.City));
                if (!string.IsNullOrWhiteSpace(query.Country))
                    q = q.Where(x =>
                        x.User.UserInfo.ShowWorkAddress && x.User.UserInfo.WorkCountry != null &&
                        x.User.UserInfo.WorkCountry.Slug.Equals(query.Country));
                if (!string.IsNullOrWhiteSpace(query.User))
                    q = q.Where(x =>
                        x.User.UserInfo.ShowEmail && x.User.Email.Contains(query.User) ||
                        x.User.Username.Contains(query.User));
                return q;
            });
        }
    }
}