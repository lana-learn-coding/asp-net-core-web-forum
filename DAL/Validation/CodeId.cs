using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DAL.Validation
{
    public class CodeId : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var code = value?.ToString();
            if (code == null) return ValidationResult.Success;
            if (!Regex.Match(code, "^[a-zA-Z1-9_]+$").Success)
            {
                return new ValidationResult(
                    $"{validationContext.DisplayName} must only contain character, number, and underscore");
            }

            return ValidationResult.Success;
        }
    }
}