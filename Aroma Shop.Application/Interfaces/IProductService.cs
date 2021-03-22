﻿using System;
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
        void AddProductsInformations(Product product, IEnumerable<string> informationsNames, IEnumerable<string> informationsValues);
        void DeleteProductInformations(Product product);
        void UpdateProductsInformations(Product product, IEnumerable<string> informationsNames, IEnumerable<string> informationsValues);
        bool UpdateProduct(AddEditProductViewModel productViewModel);
        void UpdateProductCategories(Product product, IEnumerable<int> productCategoriesId);
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        bool AddCategory(AddEditCategoryViewModel categoryViewModel);
        bool UpdateCategory(AddEditCategoryViewModel categoryViewModel);
        bool DeleteCategory(int categoryId);
        IEnumerable<SelectListItem> GetCategoriesTreeView();
        IEnumerable<SelectListItem> GetCategoriesTreeViewForEdit(Category selfCategory);
        IEnumerable<Image> GetProductImagesByIds(IEnumerable<int> productImagesIds);
        void AddProductImages(Product product, IEnumerable<IFormFile> productImages);
        void DeleteProductImages(IEnumerable<int> productImagesIds);
    }
}
