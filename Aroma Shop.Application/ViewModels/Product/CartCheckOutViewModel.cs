using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.CustomValidationAttribute;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class CartCheckOutViewModel
    {
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "نام خود را وارد نمایید")]
        public string FirstName { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "نام خانوادگی خود را وارد نمایید")]
        public string LastName { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "شماره موبایل وارد شده معتبر نیست")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره موبایل وارد شده معتبر نیست")]
        [Required(ErrorMessage = "شماره موبایل را وارد نمایید")]
        public string MobileNumber { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "استان خود را انتخاب نمایید")]
        public string UserProvince { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "شهر خود را انتخاب نمایید")]
        public string UserCity { get; set; }
        [Required(ErrorMessage = "آدرس خود را وارد نمایید")]
        public string UserAddress { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد پستی شامل 10 رقم می باشد")]
        [RegularExpression("([0-9]+)", ErrorMessage = "کد پستی تنها میتواند شامل عدد باشد")]
        [Required(ErrorMessage = "کد پستی خود را وارد نمایید")]
        public string UserZipCode { get; set; }
        public string OrderNote { get; set; }

        [Required(ErrorMessage = "لطفا روش پرداخت را انتخاب نمایید")]
        public string PaymentMethod { get; set; }
        [Compare(nameof(HiddenAcceptTheRules), ErrorMessage = "برای خرید ، باید قوانین و مقررات را بپذیرید")]
        public bool AcceptTheRules { get; set; }

        public bool HiddenAcceptTheRules { get; set; } = true;

        public Order Order { get; set; }
    }
}
