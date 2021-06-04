using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Aroma_Shop.Application.ViewModels.Account
{
    public class RegisterViewModel
    {
        [RegularExpression("^[a-z0-9]*$", ErrorMessage = "نام کاربری تنها میتواند شامل اعداد و حروف کوچک باشد")]
        [Required(ErrorMessage = "لطفا نام کاربری مورد نظر را وارد نمایید")]
        [MaxLength(75, ErrorMessage = "حداکثر 75 کارکتر مجاز می باشد")]
        [Remote("IsUserNameExist", "Account", HttpMethod = "POST"
            , AdditionalFields = "__RequestVerificationToken")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "فیلد وارد شده ایمیل نمی باشد")]
        [Required(ErrorMessage = "لطفا آدرس ایمیل خود را وارد نمایید")]
        [Remote("IsEmailExist", "Account", HttpMethod = "POST"
            , AdditionalFields = "__RequestVerificationToken")]
        public string Email { get; set; }
        [Required(ErrorMessage = "لطفا کلمه عبور مورد نظر را وارد نمایید")]
        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "حداکثر 16 کارکتر مجاز می باشد")]
        [MinLength(8, ErrorMessage = "حداقل 8 کارکتر مجاز می باشد")]
        public string Password { get; set; }
        [Required(ErrorMessage = "لطفا تکرار کلمه عبور را وارد نمایید")]
        [MaxLength(16, ErrorMessage = "حداکثر 16 کارکتر مجاز می باشد")]
        [MinLength(8, ErrorMessage = "حداقل 8 کارکتر مجاز می باشد")]
        [Compare(nameof(Password),ErrorMessage = "تکرار کلمه عبور با کلمه عبور مطابقت ندارد")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }
    }
}
