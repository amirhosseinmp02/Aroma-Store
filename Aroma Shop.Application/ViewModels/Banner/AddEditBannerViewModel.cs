using System.ComponentModel.DataAnnotations;
using Aroma_Shop.Domain.Models.CustomValidationAttribute;
using Microsoft.AspNetCore.Http;

namespace Aroma_Shop.Application.ViewModels.Banner
{
    public class AddEditBannerViewModel
    {
        [MaxLength(200,ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")] 
        public string BannerTitle { get; set; }
        public string BannerDescription { get; set; }

        [AllowedExtensions(new string[] { ".png", ".jpg", ".jpeg", ".gif" }, ErrorMessage = "تنها فرمت های png , jpg , jpeg , gif مجاز می باشند")]
        [MaxFileSize(4194304, ErrorMessage = "حداکثر حجم مجاز برای عکس بنر 4 مگابایت می باشد")]
        [Required(ErrorMessage = "عکس بنر را انتخاب کنید")]
        [DataType(DataType.Upload)]
        public IFormFile BannerImage { get; set; }

        //For Edit

        public int BannerId { get; set; }
        public string BannerCurrentImagePath { get; set; }  
    }
}
