using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.FileModels;

namespace Aroma_Shop.Domain.Models.MediaModels
{
    public class Banner
    {
        [Key]
        public int BannerId { get; set; }
        [MaxLength(200)]
        public string BannerTitle { get; set; }
        public string BannerLink { get; set; }  
        public string BannerDescription { get; set; }

        //Navigations Proterties

        [Required]
        public Image BannerImage { get; set; }
    }
}
