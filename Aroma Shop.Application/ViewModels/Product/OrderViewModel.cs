using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }   
        public string CreateTime { get; set; }
        public string PaymentTime { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderStatus { get; set; }
        public string OrderNote { get; set; }
        public string OrderTotalPrice { get; set; }
        public CustomIdentityUser OwnerUser { get; set; }   
        public ICollection<Discount> Discounts { get; set; }    
        public ICollection<OrderInvoiceDetails> OrderInvoicesDetails { get; set; }   
    }
}
