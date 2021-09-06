using System;
using System.Data.Entity;
using System.Linq;
using Core.Services.Base;
using DAL.Models.Auth;

namespace Core.Services
{
    public class UserService : SimpleCrudService<User>
    {
        public UserService(DbContext context) : base(context)
        {
        }

        public User Verify(string username, string password)
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

        public override User Create(User entity)
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

        protected override IQueryable<User> Query(IQueryable<User> queryable)
        {
            return queryable.Include("Roles");
        }

        protected override void Delete(User entity)
        {
            var defaultUser = GetForWrite(Guid.Empty.ToString());
            var posts = entity.Posts;

            foreach (var post in posts)
            {
                post.User = defaultUser;
            }

            base.Delete(entity);
        }
    }
}