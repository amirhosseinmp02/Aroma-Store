using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class AddCategoryViewModel
    {
        public Category Category { get; set; }
        public IEnumerable<SelectListItem> AllCategories { get; set; }
    }
}
