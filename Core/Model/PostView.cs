using System;

namespace Core.Model
{
    public class PostView : ViewBase
    {
        public UserViewBase User { get; set; }

        public string Content { get; set; }

        public string ThreadTitle { get; set; }

        public Guid ThreadId { get; set; }

        public int Vote { get; set; }

        public short Voted { get; set; }
    }
}