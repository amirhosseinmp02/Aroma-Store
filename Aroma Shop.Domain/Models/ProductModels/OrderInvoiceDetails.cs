using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Domain.Models.ProductModels
{
    public class OrderInvoiceDetails
    {
        [Key]
        public int OrderInvoiceId { get; set; }
        [Required]
        public bool IsInvoiceDetailsProductSimple { get; set; }
        [Required]
        public string InvoiceDetailsProductName { get; set; }   
        [Required]
        public int InvoiceDetailsTotalPrice { get; set; }
        [Required]
        public int InvoiceDetailsQuantity { get; set; }

        //Navigations Properties

        [Required]
        public Order Order { get; set; }

        public ICollection<string> InvoiceDetailsProductAttributesNames { get; set; }
        public ICollection<string> InvoiceDetailsProductVariationValues { get; set; }
    }
}
