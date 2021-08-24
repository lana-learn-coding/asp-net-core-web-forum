using System.ComponentModel.DataAnnotations;

namespace Web.Dto
{
    public class AuthPayload
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}