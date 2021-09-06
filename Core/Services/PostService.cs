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
            DefaultSort = new List<string> { "CreatedAt" };
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
            var forum = Context.Set<Forum>().First(x => x.Threads.Any(t => t.Id.Equals(entity.ThreadId)));
            if (forum == null) throw new InvalidDataException("ForumId", "Specified forum not found");

            forum.LastActivityAt = DateTime.Now;
            var isAdmin = _httpContext.User.IsInRole("Admin");
            if (forum.ForumAccess >= AccessMode.Internal && !isAdmin) throw new ForbiddenException();
            if (forum.ThreadAccess >= AccessMode.Internal && !isAdmin) throw new ForbiddenException();
        }

        protected override IQueryable<PostView> Query(IQueryable<Post> queryable)
        {
            if (!_httpContext.User.Identity?.IsAuthenticated ?? true)
                queryable = queryable.Where(x => x.Thread.Forum.ForumAccess == AccessMode.Public);
            else if (!_httpContext.User.IsInRole("Admin"))
                queryable = queryable.Where(x => x.Thread.Forum.ForumAccess < AccessMode.Internal);

            return queryable
                .Include("Forum")
                .Where(x => !x.IsOrigin)
                .ProjectTo<PostView>(_mapperConfig);
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