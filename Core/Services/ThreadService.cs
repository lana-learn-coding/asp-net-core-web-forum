using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Helper;
using Core.Model;
using Core.Model.Write;
using Core.Services.Base;
using DAL.Models;
using DAL.Models.Forum;
using DAL.Models.Topic;
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


        public ThreadView Create(CreateThreadAdmin entity)
        {
            var userId = _httpContext.User.Id() ?? throw new UnauthorizedException();
            ValidateForumAccess(entity);

            var tags = Context.Set<Tag>().Where(x => entity.TagIds.Contains(x.Id)).ToList();
            var thread = new Thread
            {
                ForumId = entity.ForumId,
                Status = entity.Status,
                Priority = entity.Priority,
                Title = entity.Title,
                Tags = tags
            };

            Context.Set<Thread>().Add(thread);
            Context.Set<Post>().Add(new Post
            {
                Id = thread.Id,
                IsOrigin = true,
                Content = entity.Content,
                ThreadId = thread.Id,
                UserId = userId
            });
            Context.SaveChanges();
            return Get(thread.Id.ToString());
        }

        public ThreadView Update(string slug, CreateThreadAdmin entity)
        {
            ValidateForumAccess(entity);
            var tags = Context.Set<Tag>().Where(x => entity.TagIds.Contains(x.Id)).ToList();
            var thread = GetForWrite(slug);
            if (_httpContext.User.IsInRole("Admin"))
            {
                thread.Status = entity.Status;
                thread.Priority = entity.Priority;
                thread.ForumId = entity.ForumId;
            }

            thread.Title = entity.Title;
            thread.Tags.Clear();
            foreach (var tag in tags)
            {
                thread.Tags.Add(tag);
            }

            var post = Context.Set<Post>().First(x => x.Id.Equals(thread.Id));
            post.Content = entity.Content;
            Context.SaveChanges();
            return Get(thread.Id.ToString());
        }

        private void ValidateForumAccess(CreateThreadAdmin entity)
        {
            var forum = Context.Set<Forum>().First(x => x.Id.Equals(entity.ForumId));
            forum.LastActivityAt = DateTime.Now;
            var isAdmin = _httpContext.User.IsInRole("Admin");
            if (forum.ForumAccess >= AccessMode.Internal && !isAdmin) throw new ForbiddenException();
            if (forum.ThreadAccess >= AccessMode.Internal && !isAdmin) throw new ForbiddenException();
            if (forum.ThreadAccess >= AccessMode.Protected && !isAdmin)
            {
                entity.Status = ThreadStatus.Pending;
                entity.Priority = (short)Priority.Normal;
            }
        }

        public override ThreadView Get(string slug)
        {
            var thread = GetForWrite(slug, q => q.Include("Forum"));
            if (thread.Forum.ForumAccess > AccessMode.Internal && !_httpContext.User.IsAdmin())
                throw new ForbiddenException();
            if (thread.Forum.ForumAccess >= AccessMode.Protected && !_httpContext.User.IsUser())
                throw new UnauthorizedException();

            thread.ViewsCount += 1;
            Context.SaveChanges();
            return base.Get(slug);
        }
    }
}