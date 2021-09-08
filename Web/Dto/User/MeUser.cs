using Core.Model;

namespace Web.Dto.User
{
    public class MeUser : UserViewBase
    {
        public string VotesCount { get; set; }

        public string ThreadsCount { get; set; }

        public string PostsCount { get; set; }
    }
}