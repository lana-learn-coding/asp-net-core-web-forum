using System.ComponentModel.DataAnnotations;

namespace DAL.Validation
{
    public class PhoneNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            if (value is string s && string.IsNullOrEmpty(s)) return ValidationResult.Success;
            return new PhoneAttribute().IsValid(value)
                ? ValidationResult.Success
                : new ValidationResult("Invalid phone number format");
        }
    }
}