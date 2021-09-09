using Core.Model;

namespace Web.Dto.User
{
    public class PublicUser : UserViewBase
    {
        public int ThreadsCount { get; set; }

        public int PostsCount { get; set; }
    }
}