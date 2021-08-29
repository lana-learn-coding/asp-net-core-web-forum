using DAL.Models.Forum;

namespace Core.Model
{
    public class ForumView : Forum
    {
        public int ThreadCounts { get; set; }
    }
}