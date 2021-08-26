using System;
using System.Collections.Generic;

namespace Web.Dto.Auth
{
    public class AuthUser
    {
        public Guid Uid { get; init; }

        public string Slug { get; init; }

        public string Avatar { get; init; }

        public string FullName { get; init; }

        public string Username { get; init; }

        public string Email { get; init; }

        public IEnumerable<string> Roles { get; init; }
    }
}