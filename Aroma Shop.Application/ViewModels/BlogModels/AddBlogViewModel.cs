using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.CustomValidationAttribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.ViewModels.BlogModels
{
    public class AddBlogViewModel
    {
        public AddBlogViewModel()
        {
            BlogCategoriesId = new List<int>();
            BlogCategories = new List<SelectListItem>();
        }

        [MaxLength(200, ErrorMessage = "حداکثر 200 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا عنوان مطلب را وارد نمایید")]
        public string BlogTitle { get; set; }
        public string BlogDescription { get; set; }
        [MaxLength(1000, ErrorMessage = "حداکثر 1000 کارکتر مجاز می باشد")]
        public string BlogShortDescription { get; set; }
        [AllowedExtensions(new string[] { ".png", ".jpg", ".jpeg", ".gif" }, ErrorMessage = "تنها فرمت های png , jpg , jpeg , gif مجاز می باشند")]
        [MaxFileSize(4194304, ErrorMessage = "حداکثر حجم مجاز برای عکس مطلب 4 مگابایت می باشد")]
        [Required(ErrorMessage = "عکس مطلب را انتخاب کنید")]
        [DataType(DataType.Upload)]
        public IFormFile BlogImage { get; set; }

        public IEnumerable<int> BlogCategoriesId { get; set; }
        public IEnumerable<SelectListItem> BlogCategories { get; set; }
    }
}
