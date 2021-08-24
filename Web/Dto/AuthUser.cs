using System;
using System.Linq;
using DAL.Models.Auth;

namespace Web.Dto
{
    public class AuthUser
    {
        public Guid Uid { get; }

        public string Slug { get; }
        public string Avatar { get; }

        public string FullName { get; }

        public string Username { get; }

        public string Email { get; }

        public string[] Roles { get; }

        public AuthUser(User from)
        {
            Slug = from.Slug;
            Uid = from.Uid;
            Avatar = from.Avatar;
            Email = from.Email;
            FullName = from.FullName;
            Username = from.Username;
            Roles = from.Roles.Select(role => role.Name).ToArray();
        }
    }
}