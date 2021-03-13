using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Aroma_Shop.Domain.Models.CustomValidationAttribute
{
    public class RequiredStrings : ValidationAttribute
    {

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var strings = value as IEnumerable<string>;
            if (strings.Count()>0)
            {
                if (strings.Any(p=>string.IsNullOrEmpty(p)))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"لطفا فیلد مورد نظر را کامل کنید";
        }
    }
}
