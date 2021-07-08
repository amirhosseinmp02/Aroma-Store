using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Internal;

namespace Aroma_Shop.Domain.Models.CustomValidationAttribute
{
    public class RegularExpressionListAttribute : RegularExpressionAttribute
    {
        public RegularExpressionListAttribute(string pattern)
            : base(pattern) { }

        public override bool IsValid(object value)
        {
            var strings = (IEnumerable<string>)value;
            if (strings.Any())
            {
                foreach (var aString in strings)
                {
                    if (!Regex.IsMatch(aString, Pattern))
                        return false;
                }
            }

            return true;
        }
    }
}
