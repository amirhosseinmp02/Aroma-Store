using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Aroma_Shop.Application.ViewModels.Account
{
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage = "فیلد وارد شده ایمیل نمی باشد")]
        [Required(ErrorMessage = "لطفا آدرس ایمیل خود را وارد نمایید")]
        public string Email { get; set; }
        [Required(ErrorMessage = "لطفا کلمه عبور خود را وارد نمایید")]
        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "حداکثر 16 کارکتر مجاز می باشد")]
        [MinLength(8, ErrorMessage = "حداقل 8 کارکتر مجاز می باشد")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
        public List<AuthenticationScheme> ExternalsLogins { get; set; }
    }
}
