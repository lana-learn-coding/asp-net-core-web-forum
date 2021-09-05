using System;
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
    public class ThreadService : CrudService<Thread, ThreadView>
    {
        private readonly HttpContext _httpContext;
        private readonly IConfigurationProvider _mapperConfig;

        public ThreadService(DbContext context, IConfigurationProvider mapperConfig,
            IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _mapperConfig = mapperConfig;
        }

        protected override IQueryable<ThreadView> Query(IQueryable<Thread> queryable)
        {
            if (!_httpContext.User.Identity?.IsAuthenticated ?? true)
                queryable = queryable.Where(x => (short)x.Forum.ForumAccess == (short)AccessMode.Public);
            else if (!_httpContext.User.IsInRole("Admin"))
                queryable = queryable.Where(x => (short)x.Forum.ForumAccess < (short)AccessMode.Internal);

            return queryable
                .Include("Forum")
                .ProjectTo<ThreadView>(_mapperConfig);
        }

        public override ThreadView Create(Thread entity)
        {
            var forum = Context.Set<Forum>().First(x => x.Id.Equals(entity.ForumId));
            forum.LastActivityAt = DateTime.Now;
            return base.Create(entity);
        }

        protected override void Update(Thread current, Thread entity)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.LastActivityAt = DateTime.Now;

            var forum = Context.Set<Forum>().First(x => x.Id.Equals(entity.ForumId));
            forum.LastActivityAt = DateTime.Now;

            base.Update(current, entity);
        }
    }
}