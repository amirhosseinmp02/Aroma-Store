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
        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا مقدار صفت را وارد نمایید")]
        public string AttributeValue { get; set; }

        //Navigations Properties

        public MixedProductAttribute MixedProductAttribute { get; set; }
        public ProductAttribute ProductAttribute { get; set; }
    }
}