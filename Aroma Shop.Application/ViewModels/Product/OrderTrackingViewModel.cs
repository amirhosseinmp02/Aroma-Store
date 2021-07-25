using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class OrderTrackingViewModel
    {
        [Required(ErrorMessage = "لطفا شناسه سفارش خود را وارد نمایید")]
        public int OrderId { get; set; }
        [EmailAddress(ErrorMessage = "فیلد وارد شده ایمیل نمی باشد")]
        [Required(ErrorMessage = "لطفا ایمیل خود را وارد نمایید")]
        public string Email { get; set; }

        public OrderViewModel Order { get; set; }
    }
}
