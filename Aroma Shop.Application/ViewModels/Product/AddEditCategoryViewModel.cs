using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class AddEditCategoryViewModel
    {
        public int CategoryId { get; set; }
        [MaxLength(100, ErrorMessage = "حداکثر 100 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام دسته را وارد نمایید")]
        public string CategoryName { get; set; }
        [MaxLength(1000, ErrorMessage = "حداکثر 1000 کارکتر مجاز می باشد")]
        public string CategoryDescription { get; set; }

        public int? ParentCategoryId { get; set; }   
        public IEnumerable<SelectListItem> AllCategories { get; set; }
    }
}
