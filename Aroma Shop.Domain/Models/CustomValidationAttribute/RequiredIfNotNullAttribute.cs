using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.CustomValidationAttribute
{
    public class RequiredIfNotNullAttribute : ValidationAttribute
    {
        private readonly RequiredAttribute _innerAttribute;
        private readonly string _principleProperty;

        public RequiredIfNotNullAttribute(string principleProperty)
        {
            _innerAttribute= new RequiredAttribute();
            _principleProperty = principleProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = value as string;
            var principleProperty = validationContext.ObjectType.GetProperty(_principleProperty);
            var principlePropertyValue = principleProperty.GetValue(validationContext.ObjectInstance, null);
            if (principlePropertyValue != null)
            {
                if (!_innerAttribute.IsValid(value))
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }
    }
}
