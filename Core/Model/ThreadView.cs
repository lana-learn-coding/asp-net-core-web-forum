using System.Text.Json.Serialization;
using DAL.Models.Forum;

namespace Core.Model
{
    public class ThreadView : Thread
    {
        public int PostsCount { get; set; }

        public UserView User => Post.User;

        public int Vote => Post.Vote;
        public string Content => Post.Content;

        [JsonIgnore]
        public PostView Post { get; set; }
    }
}