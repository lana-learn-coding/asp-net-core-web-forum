using System;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Model;
using Core.Services.Base;
using DAL.Models.Auth;

namespace Core.Services
{
    public class UserService : CrudService<User, UserView>
    {
        private readonly IConfigurationProvider _mapperConfig;

        public UserService(DbContext context, IConfigurationProvider mapperConfig) : base(context)
        {
            _mapperConfig = mapperConfig;
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

        public override UserView Create(User entity)
        {
            entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
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

            base.Update(current, entity);
        }

        protected override IQueryable<UserView> Query(IQueryable<User> queryable)
        {
            return queryable.Include("Roles")
                .ProjectTo<UserView>(_mapperConfig);
        }

        protected override void Delete(User entity)
        {
            var defaultUser = GetForWrite(Guid.Empty.ToString());
            var posts = entity.Posts;
            foreach (var post in posts) post.User = defaultUser;

            var votes = entity.Votes;
            foreach (var vote in votes) vote.User = defaultUser;
            base.Delete(entity);
        }
    }
}