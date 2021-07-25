using System.ComponentModel.DataAnnotations;
using Aroma_Shop.Domain.Models.MediaModels;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Domain.Models.FileModels
{
    public class Image
    {
        public static string DefaultImagePath { get; set; } = "/img/default-image.jpg";

        [Key]
        public int ImageId { get; set; }
        [Required]
        [MaxLength(300)]
        [Display(Name = "آپلود تصویر")]
        public string ImagePath { get; set; }

        //Navigations Proterties

        public Product Product { get; set; }
    }
}
