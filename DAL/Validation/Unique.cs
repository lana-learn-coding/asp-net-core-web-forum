using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using DAL.Models;

namespace DAL.Validation
{
    public class Unique : ValidationAttribute
    {
        public Type Type { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (ModelContext)validationContext.GetService(typeof(ModelContext));
            if (context == null) return ValidationResult.Success;
            var type = Type ?? validationContext.ObjectType;

            if (validationContext.ObjectInstance is not IIdentified identified)
            {
                return context.Set(type).Any($"{validationContext.MemberName}.Equals(@0)", value)
                    ? new ValidationResult($"The {validationContext.DisplayName} has already been taken")
                    : ValidationResult.Success;
            }

            return context.Set(type).Any($"!Slug.Equals(@0) && {validationContext.MemberName}.Equals(@1)",
                identified.Slug, value)
                ? new ValidationResult($"The {validationContext.DisplayName} has already been taken")
                : ValidationResult.Success;
        }
    }
}