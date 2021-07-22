using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Aroma_Shop.Domain.Models.CustomValidationAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NotIncludedInStringAttribute : ValidationAttribute
    {
        private string _string { get; set; }

        public NotIncludedInStringAttribute(string aString)
        {
            _string = aString;
        }

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var stringValue = value.ToString();

            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                if (stringValue.Contains(_string))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
