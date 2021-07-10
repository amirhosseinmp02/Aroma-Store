using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.ProductModels
{
    public class ProductAttributeValue
    {
        [Key]
        public int AttributeValueId { get; set; }
        [Required(ErrorMessage = "لطفا مقدار صفت را وارد نمایید")]
        public string AttributeValue { get; set; }

        //Navigations Properties

        public ProductAttribute ProductAttribute { get; set; }
    }
}