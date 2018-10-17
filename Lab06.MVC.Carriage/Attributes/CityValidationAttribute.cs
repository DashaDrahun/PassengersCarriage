using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Lab06.MVC.Carriage.UIValidation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CityValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !Regex.IsMatch(value.ToString(), @"^[a-zA-Z]+$"))
            {
                return new ValidationResult($"{validationContext.MemberName} must contain only latin letters");
            }

            return ValidationResult.Success;
        }
    }
}