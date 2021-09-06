using System;
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
            DefaultSort = new List<string> { "Priority", "LastActivityAt desc" };
            _httpContext = httpContextAccessor.HttpContext;
            _mapperConfig = mapperConfig;
        }

        public override ForumView Get(string slug)
        {
            var forum = base.Get(slug);
            if (forum.ForumAccess >= AccessMode.Internal && !_httpContext.User.IsAdmin())
                throw new ForbiddenException();
            if (forum.ForumAccess >= AccessMode.Protected && !_httpContext.User.IsUser())
                throw new UnauthorizedException();
            return forum;
        }

        protected override IQueryable<ForumView> Query(IQueryable<Forum> queryable)
        {
            if (!_httpContext.User.IsAdmin())
                queryable = queryable.Where(x => x.ForumAccess < AccessMode.Internal);

            return queryable
                .Include("Category")
                .ProjectTo<ForumView>(_mapperConfig);
        }

        protected override void Delete(Forum entity)
        {
            foreach (var thread in entity.Threads)
            {
                thread.ForumId = Guid.Empty;
            }

            base.Delete(entity);
        }
    }
}