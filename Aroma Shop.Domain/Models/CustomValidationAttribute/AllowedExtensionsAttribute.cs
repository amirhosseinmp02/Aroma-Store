using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Aroma_Shop.Domain.Models.CustomValidationAttribute
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var files = value as IEnumerable<IFormFile>;
            if (files != null)
            {
                var filesExtensions = files.Select(p => Path.GetExtension(p.FileName).ToLower());
                foreach (var fileExtension in filesExtensions)
                {
                    if (!_extensions.Contains(fileExtension))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"این نوع پسوند برای فایل مورد نظر پشتیبانی نمی شود";
        }
    }
}
