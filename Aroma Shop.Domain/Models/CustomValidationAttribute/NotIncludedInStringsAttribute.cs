using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;

namespace Aroma_Shop.Domain.Models.CustomValidationAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NotIncludedInStringsAttribute : ValidationAttribute
    {
        private string _string { get; set; }

        public NotIncludedInStringsAttribute(string aString)
        {
            _string = aString;
        }
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var strings = (IEnumerable<string>)value;

            if (strings.Any())
            {
                if (strings.Any(p => !string.IsNullOrWhiteSpace(p) && p.Contains(_string)))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
