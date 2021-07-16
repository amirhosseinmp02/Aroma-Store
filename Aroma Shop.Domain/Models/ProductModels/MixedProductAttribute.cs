using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.ProductModels
{
    public class MixedProductAttribute
    {
        [Key]
        public int MixedProductAttributeId { get; set; }
        [Required(ErrorMessage = "لطفا نام تنوع را وارد نمایید")]
        public string MixedProductAttributeValue { get; set; }
        [Required(ErrorMessage = "لطفا تغییرات قیمت محصول را وارد نمایید")]
        public int MixedProductAttributePrice { get; set; }
        [Required(ErrorMessage = "لطفا تغییرات موجودی محصول را وارد نمایید")]
        public int MixedProductAttributeQuantityInStock { get; set; }

        //Navigations Properties

        [Required]
        public Product Product { get; set; }
    }
}
