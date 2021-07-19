using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.CustomIdentityModels;

namespace Aroma_Shop.Domain.Models.ProductModels
{
    public class Order
    {
        public Order()
        {
            OrdersDetails = new List<OrderDetails>();
        }

        [Key]
        public int OrderId { get; set; }
        [Required]
        public bool IsFinally { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }

        //Navigations Properties

        public ICollection<OrderDetails> OrdersDetails { get; set; }
        [Required]
        public CustomIdentityUser OwnerUser { get; set; }
    }
}
