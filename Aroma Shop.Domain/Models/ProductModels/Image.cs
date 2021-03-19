using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Aroma_Shop.Domain.Models.ProductModels
{
    public class Image
    {
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
