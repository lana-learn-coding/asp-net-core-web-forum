using DAL.Validation;

namespace Web.Dto.User
{
    public class ChangePassword
    {
        public string Password { get; set; }

        [Password]
        public string NewPassword { get; set; }
    }
}