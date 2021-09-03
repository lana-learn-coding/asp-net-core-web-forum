using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using DAL.Models;

namespace DAL.Validation
{
    public class Unique : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (ModelContext)validationContext.GetService(typeof(ModelContext));
            if (context == null) return ValidationResult.Success;
            if (validationContext.ObjectInstance is not IIdentified identified) return ValidationResult.Success;

            var type = validationContext.ObjectType;
            var exist = context.Set(type)
                .Any($"!Id.Equals(@0) && {validationContext.MemberName}.Equals(@1)", identified.Id, value);
            return exist
                ? new ValidationResult($"The {validationContext.DisplayName} has already been taken")
                : ValidationResult.Success;
        }
    }
}