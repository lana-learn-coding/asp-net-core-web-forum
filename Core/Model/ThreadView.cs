using DAL.Models.Forum;

namespace Core.Model
{
    public class ThreadView : Thread
    {
        public int PostCounts { get; set; }

        public PostView Post { get; set; }
    }
}