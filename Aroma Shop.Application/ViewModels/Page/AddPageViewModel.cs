using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Aroma_Shop.Application.ViewModels.Page
{
    public class AddPageViewModel
    {
        public int PageId { get; set; }
        [MaxLength(300, ErrorMessage = "حداکثر 300 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا عنوان صفحه را وارد نمایید")]
        public string PageTitle { get; set; }
        [MaxLength(300, ErrorMessage = "حداکثر 500 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا آدرس صفحه را وارد نمایید")]
        [RegularExpression("^[A-Za-z0-9آابپتثجچحخدذرزژسشصضطظعغفقکگلمنوهی/-]+$", ErrorMessage = "آدرس صفحه تنها میتواند شامل - ، / ، اعداد و حروف فارسی و انگلیسی باشد")]
        [Remote("IsPagePathAddressExistForAdd", "Page", "Admin", HttpMethod = "POST"
            , AdditionalFields = "__RequestVerificationToken")]
        public string PagePathAddress { get; set; }
        public string PageDescription { get; set; }
    }
}
