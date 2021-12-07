using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.ViewModels.BlogModels
{
    public class AddEditBlogCategoryViewModel
    {
        public int CategoryId { get; set; }
        [MaxLength(250, ErrorMessage = "حداکثر 250 کارکتر مجاز می باشد")]
        [Required(ErrorMessage = "لطفا نام دسته را وارد نمایید")]
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public int ParentCategoryId { get; set; }
        public IEnumerable<SelectListItem> AllCategories { get; set; }
    }
}
