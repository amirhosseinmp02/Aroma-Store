using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class AddCategoryViewModel
    {
        public Category Category { get; set; }
        public IEnumerable<Category> AllCategories { get; set; }
    }
}
