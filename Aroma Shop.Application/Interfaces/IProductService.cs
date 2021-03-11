using System;
using System.Collections.Generic;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int productId);
        bool AddProduct(AddEditProductViewModel productViewModel);
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        bool AddCategory(AddEditCategoryViewModel categoryViewModel);
        bool UpdateCategory(AddEditCategoryViewModel categoryViewModel);
        bool DeleteCategory(int categoryId);
        IEnumerable<SelectListItem> GetCategoriesTreeView();
        IEnumerable<SelectListItem> GetCategoriesTreeViewForEdit(Category selfCategory);
        IEnumerable<Image> AddProductImages(IEnumerable<IFormFile> productImages);
    }
}
