using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Helper;
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

        public ForumService(DbContext context, IHttpContextAccessor httpContextAccessor,
            IConfigurationProvider mapperConfig) : base(context)
        {
            DefaultSort = new List<string> { "Priority", "LastActivityAt" };
            _httpContext = httpContextAccessor.HttpContext;
            _mapperConfig = mapperConfig;
        }

        protected override IQueryable<ForumView> Query(IQueryable<Forum> queryable)
        {
            if (!_httpContext.User.IsAdmin())
                queryable = queryable.Where(x => (short)x.ForumAccess < (short)AccessMode.Internal);

            return queryable
                .Include("Category")
                .ProjectTo<ForumView>(_mapperConfig);
        }
    }
}