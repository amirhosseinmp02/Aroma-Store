using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Aroma_Shop.Domain.Models.CustomValidationAttribute
{
    public class MaxStringsLength : ValidationAttribute
    {
        private readonly int _maxStringsLength;
        public MaxStringsLength(int maxStringsLength)
        {
            _maxStringsLength = maxStringsLength;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var strings = value as IEnumerable<string>;
            if (strings.Count()>0)
            {
                if (strings.Any(p=> p?.Length>_maxStringsLength))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"حداکثر { _maxStringsLength} کارکتر مجاز می باشد";
        }
    }
}
