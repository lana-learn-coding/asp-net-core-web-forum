using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Model;
using Core.Services.Base;
using DAL.Models;
using DAL.Models.Forum;
using Microsoft.AspNetCore.Http;

namespace Core.Services
{
    public class ForumService : CrudService<Forum, ForumView>
    {
        private readonly HttpContext _httpContext;
        private readonly IConfigurationProvider _mapperConfig;

        public ForumService(DbContext context, IConfigurationProvider mapperConfig,
            IHttpContextAccessor httpContextAccessor) : base(context)
        {
            DefaultSort = new List<string> { "Priority", "LastActivityAt" };
            _mapperConfig = mapperConfig;
            _httpContext = httpContextAccessor.HttpContext;
        }

        protected override IQueryable<ForumView> Query(IQueryable<Forum> queryable)
        {
            if (!_httpContext.User.IsInRole("Admin"))
                queryable = queryable.Where(x => (short)x.ForumAccess < (short)AccessMode.Internal);

            return queryable
                .Include("Category")
                .ProjectTo<ForumView>(_mapperConfig);
        }
    }
}