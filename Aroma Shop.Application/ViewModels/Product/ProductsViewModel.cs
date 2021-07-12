using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class ProductsViewModel
    {
        public IEnumerable<Domain.Models.ProductModels.Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
