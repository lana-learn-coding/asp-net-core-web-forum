using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DAL.Validation
{
    public class Password : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value.ToString();
            if (string.IsNullOrWhiteSpace(password) || password.Length <= 6)
            {
                return new ValidationResult("Password is required and at least 6 character");
            }

            if (password.Length > 120)
            {
                return new ValidationResult("Password too long! Maximum 120 character");
            }

            if (!Regex.Match(password, "[0-9]").Success)
            {
                return new ValidationResult("Password must contain number and character");
            }

            if (!Regex.Match(password, "[a-zA-Z]").Success)
            {
                return new ValidationResult("Password must contain number and character");
            }

            return ValidationResult.Success;
        }
    }
}