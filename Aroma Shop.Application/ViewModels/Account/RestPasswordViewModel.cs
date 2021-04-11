using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aroma_Shop.Application.ViewModels.Account
{
    public class RestPasswordViewModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        [Required(ErrorMessage = "لطفا کلمه عبور جدید مورد نظر را وارد نمایید")]
        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "حداکثر 16 کارکتر مجاز می باشد")]
        [MinLength(8, ErrorMessage = "حداقل 8 کارکتر مجاز می باشد")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "لطفا تکرار کلمه عبور جدید را وارد نمایید")]
        [MaxLength(16, ErrorMessage = "حداکثر 16 کارکتر مجاز می باشد")]
        [MinLength(8, ErrorMessage = "حداقل 8 کارکتر مجاز می باشد")]
        [Compare(nameof(NewPassword), ErrorMessage = "تکرار کلمه عبور با کلمه عبور مطابقت ندارد")]
        [DataType(DataType.Password)]
        public string ReNewPassword { get; set; }
    }
}
