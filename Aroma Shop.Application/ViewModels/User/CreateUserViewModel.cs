using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.ViewModels.User
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "لطفا نام کاربری مورد نظر را وارد نمایید")]
        [RegularExpression("^[a-z0-9]*$", ErrorMessage = "نام کاربری تنها میتواند شامل اعداد و حروف کوچک باشد")]
        [MaxLength(75, ErrorMessage = "حداکثر 75 کارکتر مجاز می باشد")]
        [Remote("IsUserNameExist", "Account", "", HttpMethod = "POST"
            , AdditionalFields = "__RequestVerificationToken")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "فیلد وارد شده ایمیل نمی باشد")]
        [Required(ErrorMessage = "لطفا آدرس ایمیل خود را وارد نمایید")]
        [Remote("IsEmailExist", "Account", "", HttpMethod = "POST"
            , AdditionalFields = "__RequestVerificationToken")]
        public string Email { get; set; }
        [Required(ErrorMessage = "لطفا کلمه عبور مورد نظر را وارد نمایید")]
        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "حداکثر 16 کارکتر مجاز می باشد")]
        [MinLength(8, ErrorMessage = "حداقل 8 کارکتر مجاز می باشد")]
        public string UserPassword { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
        public string UserRoleName { get; set; }
        public string PersianUserRoleName
        {
            get
            {
                if (UserRoleName == "Founder")
                {
                    return "موسس";
                }
                else if (UserRoleName == "Manager")
                {
                    return "مدیر";
                }
                else if (UserRoleName == "Writer")
                {
                    return "نویسنده";
                }
                else
                {
                    return "مشتری";
                }
            }
        }
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

        //Properties For Editing Product

        public string UserId { get; set; }
    }
}
