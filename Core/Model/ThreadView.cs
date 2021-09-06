using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using DAL.Models;
using DAL.Models.Forum;
using DAL.Models.Topic;

namespace Core.Model
{
    public class ThreadView : ViewBase
    {
        public int PostsCount { get; set; }

        public UserView User => Post.User;

        public string Content => Post.Content;

        [JsonIgnore]
        public PostView Post { get; set; }

        public string Title { get; set; }

        public Guid ForumId { get; set; }

        public ThreadForumView Forum { get; set; }

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public DateTime LastActivityAt { get; set; }

        public int ViewsCount { get; set; }

        public short Priority { get; set; }

        public ThreadStatus Status { get; set; }

        public List<Guid> TagIds => Tags.Select(x => x.Id).ToList();
    }

    public class ThreadForumView : ViewBase
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public AccessMode ThreadAccess { get; set; }

        public AccessMode ForumAccess { get; set; }
    }
}