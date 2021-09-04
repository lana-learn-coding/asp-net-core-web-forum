namespace Core.Model
{
    public class PostView : ViewBase
    {
        public UserView User { get; set; }
        public int Vote { get; set; }
    }
}