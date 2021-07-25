using System;
using System.Collections.Generic;
using System.Text;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class OrdersViewModel
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        //If Order Is Completed The OrderDate Is Payment RegistritionDate Otherwise The OrderDate Is Order Create Date
        public string OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderTotalPrice { get; set; }
        public bool IsOrderSeen { get; set; }   
        public bool NotEmpty { get; set; }  
    }
}
