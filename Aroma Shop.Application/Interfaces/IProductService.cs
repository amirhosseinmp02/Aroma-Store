﻿using System;
using System.Collections.Generic;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int productId);
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        bool AddCategory(AddCategoryViewModel categoryViewModel);
        IEnumerable<SelectListItem> GetCategoriesTreeView(IEnumerable<Category> categories);
    }
}
