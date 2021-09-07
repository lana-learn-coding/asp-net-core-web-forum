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
    public class PostService : CrudService<Post, PostView>
    {
        private readonly HttpContext _httpContext;

        private readonly IConfigurationProvider _mapperConfig;

        public PostService(DbContext context, IHttpContextAccessor httpContextAccessor,
            IConfigurationProvider mapperConfig) : base(context)
        {
            DefaultSort = new List<string> { "CreatedAt desc" };
            _httpContext = httpContextAccessor.HttpContext;
            _mapperConfig = mapperConfig;
        }

        public override PostView Create(Post entity)
        {
            var userId = _httpContext.User.Id() ?? throw new UnauthorizedException();
            entity.UserId = userId;
            ValidateForumAccess(entity);
            return base.Create(entity);
        }

        private void ValidateForumAccess(Post entity)
        {
            var thread = Context.Set<Thread>().First(x => x.Id.Equals(entity.ThreadId));
            if (thread == null) throw new InvalidDataException("ThreadId", "Specified thread not found");
            if (thread.Status != ThreadStatus.Approved)
                throw new InvalidDataException("ThreadId", "Thread is not active");

            var forum = Context.Set<Forum>().First(x => x.Threads.Any(t => t.Id.Equals(entity.ThreadId)));
            if (forum == null) throw new InvalidDataException("ThreadId", "Specified forum not found");

            forum.LastActivityAt = DateTime.Now;
            var isAdmin = _httpContext.User.IsInRole("Admin");
            if (forum.ForumAccess >= AccessMode.Internal && !isAdmin) throw new ForbiddenException();
        }

        protected override IQueryable<PostView> Query(IQueryable<Post> queryable)
        {
            if (!_httpContext.User.Identity?.IsAuthenticated ?? true)
                queryable = queryable.Where(x => x.Thread.Forum.ForumAccess == AccessMode.Public);
            else if (!_httpContext.User.IsInRole("Admin"))
                queryable = queryable.Where(x => x.Thread.Forum.ForumAccess < AccessMode.Internal);

            var authId = _httpContext.User.Id() ?? Guid.NewGuid();
            return queryable
                .Include("Forum")
                .Where(x => !x.Id.Equals(x.ThreadId))
                .ProjectTo<PostView>(_mapperConfig, new { authId });
        }

        public void Vote(string slug, short val)
        {
            var userId = _httpContext.User.Id() ?? throw new UnauthorizedException();

            var post = GetForWrite(slug, q => q.Include("Thread.Forum"));
            if (post.Thread.Forum.ForumAccess > AccessMode.Internal && !_httpContext.User.IsAdmin())
                throw new ForbiddenException();

            // user vote for post should always 1, except for deleted user (which is default user Anon)
            if (!userId.Equals(Guid.Empty))
            {
                var currentUserVotes = post.Votes.Where(x => x.User.Id.Equals(userId)).ToList();
                foreach (var vote in currentUserVotes)
                {
                    post.Votes.Remove(vote);
                }
            }

            if (val != 0)
            {
                post.Votes.Add(new Vote
                {
                    UserId = userId,
                    Post = post,
                    Value = (short)(val > 0 ? 1 : -1)
                });
            }

            Context.SaveChanges();
        }

        public override PostView Get(string slug)
        {
            var post = GetForWrite(slug, q => q.Include("Thread.Forum"));
            if (post.Thread.Forum.ForumAccess > AccessMode.Internal && !_httpContext.User.IsAdmin())
                throw new ForbiddenException();
            if (post.Thread.Forum.ForumAccess >= AccessMode.Protected && !_httpContext.User.IsUser())
                throw new UnauthorizedException();
            return base.Get(slug);
        }
    }
}