using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.ProductModels
{
    public class ProductAttribute
    {
        [Key]
        public int ProductAttributeId { get; set; }
        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام صفت را وارد نمایید")]
        public string ProductAttributeName { get; set; }

        //Navigations Properties

        [Required]
        public Product Product { get; set; }
        [Required]
        public ICollection<ProductAttributeValue> ProductAttributeValues { get; set; }
    }
}
