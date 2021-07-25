using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Aroma_Shop.Application.ViewModels.Banner
{
    public class AddEditBannerViewModel
    {
        [MaxLength(200)]
        public string BannerTitle { get; set; }
        public string BannerDescription { get; set; }

        [Required]
        public IFormFile BannerImage { get; set; }  
    }
}
