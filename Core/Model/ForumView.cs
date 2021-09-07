using System;
using DAL.Models;
using DAL.Models.Topic;

namespace Core.Model
{
    public class ForumView : ViewBase
    {
        public int ThreadsCount { get; set; }

        public int PostsCount { get; set; }

        public LastThread LastThread { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public AccessMode ThreadAccess { get; set; }

        public AccessMode ForumAccess { get; set; }

        public DateTime LastActivityAt { get; set; }

        public short Priority { get; set; }

        public int ViewsCount { get; set; }
    }

    public class LastThread : ViewBase
    {
        public UserViewBase User { get; set; }

        public string Title { get; set; }

        public DateTime LastActivityAt { get; set; }
    }
}