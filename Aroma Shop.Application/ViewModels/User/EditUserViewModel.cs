using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.ViewModels.User
{
    public class EditUserViewModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "لطفا نام کاربری مورد نظر را وارد نمایید")]
        [MaxLength(75, ErrorMessage = "حداکثر 75 کارکتر مجاز می باشد")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "فیلد وارد شده ایمیل نمی باشد")]
        [Required(ErrorMessage = "لطفا آدرس ایمیل خود را وارد نمایید")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "حداکثر 16 کارکتر مجاز می باشد")]
        [MinLength(8, ErrorMessage = "حداقل 8 کارکتر مجاز می باشد")]
        public string UserPassword { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
        public string UserRole { get; set; }
        [MaxLength(150, ErrorMessage = "حداکثر 150 کارکتر مجاز می باشد")]
        public string FirstName { get; set; }
        [MaxLength(150, ErrorMessage = "حداکثر 150 کارکتر مجاز می باشد")]
        public string LastName { get; set; }
        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        public string UserProvince { get; set; }
        [MaxLength(150, ErrorMessage = "حداکثر 150 کارکتر مجاز می باشد")]
        public string UserCity { get; set; }
        public string UserAddress { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد پستی شامل 10 رقم می باشد")]
        public string UserZipCode { get; set; }
    }
}
