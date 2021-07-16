using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.ProductModels
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsId { get; set; }
        [Required]
        public bool IsOrderDetailsProductSimple { get; set; } 
        [Required]
        public int OrderDetailsTotalPrice { get; set; }
        [Required]
        public int OrderDetailsQuantity { get; set; }

        //Navigations Properties

        [Required]
        public Order Order { get; set; }
        [Required]
        public Product Product { get; set; }

        public ProductVariation? ProductVariation { get; set; }  
    }
}
