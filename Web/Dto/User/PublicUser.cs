using Core.Model;

namespace Web.Dto.User
{
    public class PublicUser : UserViewBase
    {
        public string ThreadsCount { get; set; }

        public string PostsCount { get; set; }
    }
}