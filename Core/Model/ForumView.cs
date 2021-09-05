using System;
using DAL.Models.Forum;

namespace Core.Model
{
    public class ForumView : Forum
    {
        public int ThreadsCount { get; set; }

        public int PostsCount { get; set; }

        public LastThread LastThread { get; set; }
    }

    public class LastThread : ViewBase
    {
        public UserView User { get; set; }

        public string Title { get; set; }

        public DateTime LastActivityAt { get; set; }
    }
}