using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Aroma_Shop.Domain.Models.CustomValidationAttribute
{
    public class MaxFilesCountAttribute : ValidationAttribute   
    {
        private readonly int _maxFileCount;
        public MaxFilesCountAttribute(int maxFileCount)
        {
            _maxFileCount = maxFileCount;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var files = value as IEnumerable<IFormFile>;
            if (files != null)
            {
                if (files.Count()>_maxFileCount)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"حداکثر { _maxFileCount} فایل مجاز می باشد";
        }
    }
}
