using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class AddEditCategoryViewModel
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
