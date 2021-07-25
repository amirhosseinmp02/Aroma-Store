using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Aroma_Shop.Application.ViewModels.Banner
{
    public class AddEditBannerViewModel
    {
        [MaxLength(200)]
        public string BannerTitle { get; set; }
        public string BannerDescription { get; set; }

        [FileExtensions(Extensions = ".png,.jpg,.jpeg,.gif", ErrorMessage = "تنها فرمت های png , jpg , jpeg , gif مجاز می باشند")]
        [MaxLength(4194304,ErrorMessage = "حداکثر حجم مجاز برای عکس بنر 4 مگابایت می باشد")]
        [Required(ErrorMessage = "عکس بنر را انتخاب کنید")]
        public IFormFile BannerImage { get; set; }  
    }
}
