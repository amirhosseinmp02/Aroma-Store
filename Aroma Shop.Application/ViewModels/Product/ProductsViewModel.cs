using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.ViewModels.Product
{
    public class ProductsViewModel
    {
        public ProductsViewModel()
        {
            SortList = new List<SelectListItem>()
            {
                new SelectListItem("مرتب سازی بر اساس جدیدترین", "Newest"),
                new SelectListItem("مرتب سازی بر اساس محبوبیت","Popularity"),
                new SelectListItem("مرتب سازی بر اساس ارزانترین","Price-Cheapest"),
                new SelectListItem("مرتب سازی بر اساس گرانترین","Price-Most-Expensive")
            };
        }

        public IEnumerable<Domain.Models.ProductModels.Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<SelectListItem> SortList { get; set; }
        public string SortBy { get; set; }
        public IEnumerable<int> SelectedCategories { get; set; }
    }
}
