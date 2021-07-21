using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using Aroma_Shop.Domain.Models.CustomValidationAttribute;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.ViewModels.Account
{
    public class EditAccountViewModel
    {
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        public string FirstName { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "لطفا نام کاربری مورد نظر را وارد نمایید")]
        [RegularExpression("^[a-z0-9]*$", ErrorMessage = "نام کاربری تنها میتواند شامل اعداد و حروف کوچک باشد")]
        [MaxLength(75, ErrorMessage = "حداکثر 75 کارکتر مجاز می باشد")]
        public string UserName { get; set; }
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "شماره موبایل وارد شده معتبر نیست")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره موبایل وارد شده معتبر نیست")]
        public string MobileNumber { get; set; }

        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        public string UserProvince { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [RequiredIfNotNull(nameof(UserProvince), ErrorMessage = "در صورت انتخاب استان ، شهر خود را هم انتخاب نمایید")]
        public string UserCity { get; set; }
        public string UserAddress { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد پستی شامل 10 رقم می باشد")]
        [RegularExpression("([0-9]+)",ErrorMessage = "کد پستی تنها میتواند شامل عدد باشد")]
        public string UserZipCode { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "حداکثر 16 کارکتر مجاز می باشد")]
        [MinLength(8, ErrorMessage = "حداقل 8 کارکتر مجاز می باشد")]
        [RequiredIfNotNull(nameof(UserNewPassword), ErrorMessage = "کلمه عبور فعلی را نیز وارد نمایید")]
        public string UserCurrentPassword { get; set; }
        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "حداکثر 16 کارکتر مجاز می باشد")]
        [MinLength(8, ErrorMessage = "حداقل 8 کارکتر مجاز می باشد")]
        [RequiredIfNotNull(nameof(UserCurrentPassword), ErrorMessage = "کلمه عبور جدید را وارد نمایید")]
        public string UserNewPassword { get; set; }
        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "حداکثر 16 کارکتر مجاز می باشد")]
        [MinLength(8, ErrorMessage = "حداقل 8 کارکتر مجاز می باشد")]
        [Compare(nameof(UserNewPassword), ErrorMessage = "تکرار کلمه عبور با کلمه عبور مطابقت ندارد")]
        public string ReNewPassword { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "حداکثر 16 کارکتر مجاز می باشد")]
        [MinLength(8, ErrorMessage = "حداقل 8 کارکتر مجاز می باشد")]
        public string UserFirstPassword { get; set; }
        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "حداکثر 16 کارکتر مجاز می باشد")]
        [MinLength(8, ErrorMessage = "حداقل 8 کارکتر مجاز می باشد")]
        [Compare(nameof(UserFirstPassword), ErrorMessage = "تکرار کلمه عبور با کلمه عبور مطابقت ندارد")]
        public string ReFirstPassword { get; set; }
    }
}
