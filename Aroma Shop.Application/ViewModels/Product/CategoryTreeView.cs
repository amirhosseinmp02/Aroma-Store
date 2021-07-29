using System;
using System.Collections.Generic;
using System.Text;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class CategoryTreeView
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryLevel { get; set; }
        public int CategoryProductsCount { get; set; }  
    }
}
