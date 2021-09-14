using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Helper;
using Core.Model;
using Core.Services.Base;
using DAL.Models.Auth;
using Microsoft.AspNetCore.Http;

namespace Core.Services
{
    public class UserService : CrudService<User, UserView>
    {
        private readonly HttpContext _httpContext;
        private readonly IConfigurationProvider _mapperConfig;
        private readonly MailService _mailService;

        public UserService(DbContext context, IConfigurationProvider mapperConfig,
            IHttpContextAccessor httpContextAccessor, MailService mailService) : base(context)
        {
            DefaultSort = new List<string> { "UpdatedAt desc" };
            _mapperConfig = mapperConfig;
            _httpContext = httpContextAccessor.HttpContext;
            _mailService = mailService;
        }

        public UserView Verify(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new UnauthorizedException();
            }

            var user = Find(query =>
                query.Where(user => user.Username.Equals(username) || user.Email.Equals(username)), true);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new UnauthorizedException();
            }

            return user;
        }

        public string ConfirmEmail(string token)
        {
            var user = DbSet.FirstOrDefault(x => x.EmailConfirmToken.Equals(token)) ??
                       throw new InvalidDataException("token", "Invalid token");
            user.EmailConfirmToken = string.Empty;
            Context.SaveChanges();
            return user.Email;
        }

        public override UserView Create(User entity)
        {
            entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
            if (_httpContext.User.IsAdmin())
            {
                FillRoles(entity);
                // always confirmed if account created by admin
                entity.EmailConfirmToken = string.Empty;
            }
            else
            {
                entity.EmailConfirmToken = _mailService.SendEmailConfirmMail(entity.Email, entity.Username);
            }

            entity.UserInfo = new UserInfo();
            return base.Create(entity);
        }

        protected override void Update(User current, User entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Password) || entity.Password.Equals(current.Password))
            {
                entity.Password = current.Password;
            }
            else
            {
                entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
            }

            if (_httpContext.User.IsAdmin())
            {
                current.RoleIds = entity.RoleIds;
                FillRoles(current);
            }

            base.Update(current, entity);
        }

        public void ChangePassword(string slug, string newPassword)
        {
            var update = FindForWrite(slug, x => x.Include("Roles"));
            update.RoleIds = update.Roles.Select(x => x.Id).ToList();
            update.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            base.Update(slug, update);
        }

        public void ChangeAvatar(string slug, string newAvatar)
        {
            var update = FindForWrite(slug, x => x.Include("Roles"));
            update.RoleIds = update.Roles.Select(x => x.Id).ToList();
            update.Avatar = newAvatar;
            base.Update(slug, update);
        }

        private void FillRoles(User user)
        {
            var roleIds = user.RoleIds;
            if (user.RoleIds == null || roleIds.Count == 0)
            {
                user.Roles.Clear();
                return;
            }

            var roles = Context.Set<Role>().Where(x => roleIds.Contains(x.Id));
            user.Roles.Clear();
            foreach (var role in roles) user.Roles.Add(role);
        }

        protected override IQueryable<UserView> Query(IQueryable<User> queryable)
        {
            if (!_httpContext.User.IsAdmin())
                queryable = queryable.Where(x => !x.Id.Equals(Guid.Empty));

            return queryable.Include("Roles")
                .Include("UserInfo")
                .ProjectTo<UserView>(_mapperConfig);
        }

        protected override void Delete(User entity)
        {
            var defaultUser = FindForWrite(Guid.Empty.ToString());
            var posts = entity.Posts;
            foreach (var post in posts) post.User = defaultUser;

            var votes = entity.Votes;
            foreach (var vote in votes) vote.User = defaultUser;
            base.Delete(entity);
        }
    }
}