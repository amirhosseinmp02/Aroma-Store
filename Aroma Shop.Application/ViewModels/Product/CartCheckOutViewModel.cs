using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.CustomValidationAttribute;

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
        public string? MobileNumber { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        public string UserProvince { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [RequiredIfNotNull(nameof(UserProvince), ErrorMessage = "در صورت انتخاب استان ، شهر خود را هم انتخاب نمایید")]
        public string UserCity { get; set; }
        public string UserAddress { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد پستی شامل 10 رقم می باشد")]
        [RegularExpression("([0-9]+)", ErrorMessage = "کد پستی تنها میتواند شامل عدد باشد")]
        public string UserZipCode { get; set; }
        public string OrderNote { get; set; }   
    }
}
