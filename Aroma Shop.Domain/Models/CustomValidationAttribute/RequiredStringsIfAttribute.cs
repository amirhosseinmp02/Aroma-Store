using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Aroma_Shop.Domain.Models.CustomValidationAttribute
{
    public class RequiredStringsIfAttribute : ValidationAttribute
    {
        private string _principleProperty { get; set; }
        private object _principlePropertyDesiredValue { get; set; }

        public RequiredStringsIfAttribute(String principleProperty, Object principlePropertyDesiredValue)
        {
            _principleProperty = principleProperty;
            _principlePropertyDesiredValue = principlePropertyDesiredValue;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();
            var propertyValue = type.GetProperty(_principleProperty)?.GetValue(instance, null);
            if (propertyValue?.ToString() == _principlePropertyDesiredValue.ToString())
            {
                var strings = value as IEnumerable<string>;
                if (strings.Any())
                {
                    if (strings.Any(p => string.IsNullOrEmpty(p)))
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
                else
                {
                    return new ValidationResult(GetErrorMessageForNullList());
                }
            }
            
            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"لطفا فیلد مورد نظر را کامل کنید";
        }

        public string GetErrorMessageForNullList()
        {
            return "لطفا بخش مورد نظر را کامل کنید";
        }
    }
}
