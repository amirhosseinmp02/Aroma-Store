using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.ProductModels
{
    public class ProductVariation
    {
        [Key]
        public int ProductVariationId { get; set; }
        [Required]
        public ICollection<string> ProductVariationValues { get; set; }
        [Required(ErrorMessage = "لطفا تغییرات قیمت محصول را وارد نمایید")]
        public int ProductVariationPrice { get; set; }
        [Required(ErrorMessage = "لطفا تغییرات موجودی محصول را وارد نمایید")]
        public int ProductVariationQuantityInStock { get; set; }

        //Navigations Properties

        [Required]
        public Product Product { get; set; }
    }
}
