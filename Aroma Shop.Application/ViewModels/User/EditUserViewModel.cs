using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.CustomValidationAttribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.ViewModels.User
{
    public class EditUserViewModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "لطفا نام کاربری مورد نظر را وارد نمایید")]
        [RegularExpression("^[a-z0-9]*$", ErrorMessage = "نام کاربری تنها میتواند شامل اعداد و حروف کوچک باشد")]
        [MaxLength(75, ErrorMessage = "حداکثر 75 کارکتر مجاز می باشد")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "فیلد وارد شده ایمیل نمی باشد")]
        [Required(ErrorMessage = "لطفا آدرس ایمیل خود را وارد نمایید")]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "شماره موبایل وارد شده معتبر نیست")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره موبایل وارد شده معتبر نیست")]
        public string MobileNumber { get; set; }
        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "حداکثر 16 کارکتر مجاز می باشد")]
        [MinLength(8, ErrorMessage = "حداقل 8 کارکتر مجاز می باشد")]
        public string UserPassword { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        public string UserRoleName { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        public string FirstName { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        public string LastName { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        public string UserProvince { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [RequiredIfNotNull(nameof(UserProvince), ErrorMessage = "در صورت انتخاب استان ، شهر خود را هم انتخاب نمایید")]
        public string UserCity { get; set; }
        public string UserAddress { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد پستی شامل 10 رقم می باشد")]
        public string UserZipCode { get; set; }
    }
}
