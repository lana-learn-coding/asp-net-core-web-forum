using System;

namespace Core.Model
{
    public class PostView : ViewBase
    {
        public UserView User { get; set; }

        public string Content { get; set; }

        public string ThreadTitle { get; set; }

        public Guid ThreadId { get; set; }
    }
}