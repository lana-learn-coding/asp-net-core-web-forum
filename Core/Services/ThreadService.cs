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
    public class ThreadService : CrudService<Thread, ThreadView>
    {
        private readonly HttpContext _httpContext;

        private readonly IConfigurationProvider _mapperConfig;

        public ThreadService(DbContext context, IHttpContextAccessor httpContextAccessor,
            IConfigurationProvider mapperConfig) : base(context)
        {
            DefaultSort = new List<string> { "Priority", "LastActivityAt" };
            _httpContext = httpContextAccessor.HttpContext;
            _mapperConfig = mapperConfig;
        }

        protected override IQueryable<ThreadView> Query(IQueryable<Thread> queryable)
        {
            if (!_httpContext.User.Identity?.IsAuthenticated ?? true)
                queryable = queryable.Where(x => x.Forum.ForumAccess == AccessMode.Public);
            else if (!_httpContext.User.IsInRole("Admin"))
                queryable = queryable.Where(x => x.Forum.ForumAccess < AccessMode.Internal)
                    .Where(x => x.Status == ThreadStatus.Approved);

            return queryable
                .Include("Forum")
                .ProjectTo<ThreadView>(_mapperConfig);
        }

        public override ThreadView Create(Thread entity)
        {
            var forum = Context.Set<Forum>().First(x => x.Id.Equals(entity.ForumId));
            if (!_httpContext.User.IsUser()) throw new UnauthorizedException();

            var isAdmin = _httpContext.User.IsInRole("Admin");
            if (forum.ForumAccess >= AccessMode.Internal && !isAdmin) throw new ForbiddenException();
            if (forum.ThreadAccess >= AccessMode.Internal && !isAdmin) throw new ForbiddenException();
            if (forum.ThreadAccess >= AccessMode.Protected && !isAdmin)
            {
                entity.Status = ThreadStatus.Pending;
                entity.Priority = (short)Priority.Normal;
            }

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

        // 
        public override ThreadView Get(string slug)
        {
            var thread = base.Get(slug);
            if (thread.Forum.ForumAccess > AccessMode.Internal && !_httpContext.User.IsAdmin())
                throw new ForbiddenException();
            if (thread.Forum.ForumAccess > AccessMode.Protected && !_httpContext.User.IsUser())
                throw new UnauthorizedException();

            thread.ViewsCount += 1;
            Context.SaveChangesAsync();
            return thread;
        }
    }
}