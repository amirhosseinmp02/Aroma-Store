using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.CustomValidationAttribute;
using Microsoft.AspNetCore.Http;

namespace Aroma_Shop.Application.ViewModels.Banner
{
    public class EditBannerViewModel
    {
        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        public string BannerTitle { get; set; }
        [RegularExpression("(https?:\\/\\/(?:www\\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\\.[^\\s]{2,}|www\\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\\.[^\\s]{2,}|https?:\\/\\/(?:www\\.|(?!www))[a-zA-Z0-9]+\\.[^\\s]{2,}|www\\.[a-zA-Z0-9]+\\.[^\\s]{2,})", ErrorMessage = "پیوند وارد شده معتبر نیست")]
        public string BannerLink { get; set; }
        public string BannerDescription { get; set; }

        [AllowedExtensions(new string[] { ".png", ".jpg", ".jpeg", ".gif" }, ErrorMessage = "تنها فرمت های png , jpg , jpeg , gif مجاز می باشند")]
        [MaxFileSize(4194304, ErrorMessage = "حداکثر حجم مجاز برای عکس بنر 4 مگابایت می باشد")]
        [DataType(DataType.Upload)]
        public IFormFile BannerImage { get; set; }
        [Required(ErrorMessage = "لطفا مکان قرار گیری بنر را انتخاب نمایید")]
        public bool IsPrimaryBanner { get; set; }

        //For Edit

        public int BannerId { get; set; }
        public string BannerCurrentImagePath { get; set; }
    }
}
