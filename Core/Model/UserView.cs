using System.Collections.Generic;
using DAL.Models.Auth;

namespace Core.Model
{
    public class UserView : UserViewBase
    {
        public string Password { get; set; }

        public string VotesCount { get; set; }

        public string ThreadsCount { get; set; }

        public string PostsCount { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}