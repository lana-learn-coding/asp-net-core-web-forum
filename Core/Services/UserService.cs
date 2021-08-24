using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
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
            ValidatePassword(entity);
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
                ValidatePassword(entity);
                entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
            }

            base.Update(current, entity);
        }

        private static void ValidatePassword(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length <= 6)
            {
                throw new InvalidDataException("Password", "Password is required and at least 6 character");
            }

            if (!Regex.Match(user.Password, ".*[1-9].*[a-zA-Z].*").Success)
            {
                throw new InvalidDataException("Password", "Password must contain number and character");
            }
        }

        protected override IQueryable<User> Query(DbSet<User> dbSet)
        {
            return dbSet.Include("Roles");
        }
    }
}