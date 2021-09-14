using System.Text.Json.Serialization;

namespace Core.Model
{
    public class UserViewBase : ViewBase
    {
        public virtual string Avatar { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }

        [JsonIgnore]
        public virtual bool IsEmailConfirmed { get; set; }
    }
}