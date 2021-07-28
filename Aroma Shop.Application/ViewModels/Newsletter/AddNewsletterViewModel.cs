using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Aroma_Shop.Application.ViewModels.Newsletter
{
    public class AddNewsletterViewModel
    {
        [EmailAddress(ErrorMessage = "فیلد وارد شده ایمیل نمی باشد")]
        [MaxLength(256, ErrorMessage = "حداکثر 256 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا آدرس ایمیل خود را وارد نمایید")]
        [Remote("IsNewsletterEmailExist", "Media", "Admin", HttpMethod = "POST"
            , AdditionalFields = "__RequestVerificationToken,")]
        public string CustomerEmail { get; set; }
    }
}
