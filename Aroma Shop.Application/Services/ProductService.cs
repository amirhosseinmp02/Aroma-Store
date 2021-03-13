using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Product GetProduct(int productId)
        {
            return _productRepository.GetProduct(productId);
        }

        public bool AddProduct(AddEditProductViewModel productViewModel)
        {
            try
            {
                var productCategories = new List<Category>();
                foreach (var productCategoryId in productViewModel.ProductCategoriesId)
                {
                    productCategories.Add(GetCategory(productCategoryId));
                }

                var productsInformations = new List<ProductInformation>();
                if (productViewModel.InformationsNames.Count() > 0 && productViewModel.InformationsValues.Count() > 0)
                {
                    for (int i = 0; i < productViewModel.InformationsNames.Count(); i++)
                    {
                        if (string.IsNullOrEmpty(productViewModel.InformationsNames.ElementAtOrDefault(i))
                            || string.IsNullOrEmpty(productViewModel.InformationsValues.ElementAtOrDefault(i)))
                            break;
                        var productInformations = new ProductInformation()
                        {
                            Name = productViewModel.InformationsNames.ElementAtOrDefault(i),
                            Value = productViewModel.InformationsValues.ElementAtOrDefault(i)
                        };
                        productsInformations.Add(productInformations);
                    }
                }

                var product = new Product()
                {
                    ProductName = productViewModel.ProductName,
                    ProductDescription = productViewModel.ProductDescription,
                    ProductPrice = productViewModel.ProductPrice,
                    ProductQuantityInStock = productViewModel.ProductQuantityInStock,
                    Categories = productCategories,
                    Informations = productsInformations
                };
                AddProductImages(product,productViewModel.ProductImages);
                _productRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetProducts();
        }

        public IEnumerable<Category> GetCategories()
        {
            return _productRepository.GetCategories();
        }

        public Category GetCategory(int categoryId)
        {
            return _productRepository.GetCategory(categoryId);
        }

        public bool AddCategory(AddEditCategoryViewModel categoryViewModel)
        {
            try
            {
                var category = new Category()
                {
                    CategoryName = categoryViewModel.CategoryName,
                    CategoryDescription = categoryViewModel.CategoryDescription
                };
                if (categoryViewModel.ParentCategoryId != null)
                {
                    var parentCategory =
                        _productRepository
                            .GetCategory((int)categoryViewModel.ParentCategoryId);
                    category.ParentCategory = parentCategory;
                }
                _productRepository.AddCategory(category);
                _productRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool UpdateCategory(AddEditCategoryViewModel categoryViewModel)
        {
            try
            {
                var parentCategory =
                    GetCategory(Convert.ToInt32(categoryViewModel.ParentCategoryId));
                var category = GetCategory(categoryViewModel.CategoryId);
                category.CategoryName = categoryViewModel.CategoryName;
                category.CategoryDescription = categoryViewModel.CategoryDescription;
                category.ParentCategory = parentCategory;
                _productRepository.UpdateCategory(category);
                _productRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCategory(int categoryId)
        {
            try
            {
                _productRepository.DeleteCategory(categoryId);
                _productRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<SelectListItem> GetCategoriesTreeView()
        {
            var categories = _productRepository.GetCategories();
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem("انتخاب کنید", ""));
            var parentsCategories =
                categories.Where(p => p.ParentCategory == null);
            int count = 0;
            ParentsCategoriesScrolling(parentsCategories);
            void ParentsCategoriesScrolling(IEnumerable<Category> parents)
            {
                foreach (var parent in parents)
                {
                    items.Add(new SelectListItem(new string('─', count * 2) + $" {parent.CategoryName}", parent.CategoryId.ToString()));
                    var category = _productRepository.GetCategory(parent.CategoryId);
                    if (category.ChildrenCategories.Count != 0)
                    {
                        ++count;
                        ChildrenCategoriesScrolling(category.ChildrenCategories, count);
                    }
                    count = 0;
                }
            }
            void ChildrenCategoriesScrolling(IEnumerable<Category> children, int counter)
            {
                foreach (var child in children)
                {
                    items.Add(new SelectListItem(new string('─', counter * 2) + $" {child.CategoryName}", child.CategoryId.ToString()));
                    var category = _productRepository.GetCategory(child.CategoryId);
                    if (category.ChildrenCategories.Count != 0)
                    {
                        ++counter;
                        ChildrenCategoriesScrolling(category.ChildrenCategories, counter);
                        --counter;
                    }

                }
            }
            return items;
        }

        public IEnumerable<SelectListItem> GetCategoriesTreeViewForEdit(Category selfCategory)
        {
            var categories = _productRepository.GetCategories();
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem("انتخاب کنید", ""));
            var parentsCategories =
                categories.Where(p => p.ParentCategory == null);
            int count = 0;
            ParentsCategoriesScrolling(parentsCategories);
            void ParentsCategoriesScrolling(IEnumerable<Category> parents)
            {
                foreach (var parent in parents)
                {
                    if (parent.CategoryId != selfCategory.CategoryId)
                    {
                        items.Add
                            (new SelectListItem
                            (new string('─', count * 2) +
                             $" {parent.CategoryName}", parent.CategoryId.ToString()));
                        var category = _productRepository.GetCategory(parent.CategoryId);
                        if (category.ChildrenCategories.Count != 0)
                        {
                            ++count;
                            ChildrenCategoriesScrolling(category.ChildrenCategories, count);
                        }
                        count = 0;
                    }
                }
            }
            void ChildrenCategoriesScrolling(IEnumerable<Category> children, int counter)
            {
                foreach (var child in children)
                {
                    if (child.CategoryId != selfCategory.CategoryId)
                    {
                        items.Add(new SelectListItem
                            (new string('─', counter * 2) +
                             $" {child.CategoryName}", child.CategoryId.ToString()));
                        var category = _productRepository.GetCategory(child.CategoryId);
                        if (category.ChildrenCategories.Count != 0)
                        {
                            ++counter;
                            ChildrenCategoriesScrolling(category.ChildrenCategories, counter);
                            --counter;
                        }
                    }
                }
            }

            if (selfCategory.ParentCategory != null)
            {
                items
                    .SingleOrDefault
                    (p =>
                        p.Value == selfCategory.ParentCategory.CategoryId.ToString())
                    .Selected = true;
            }
            return items;
        }

        public void AddProductImages(Product product,IEnumerable<IFormFile> productImagesFiles)
        {
            var persianCalendar = new PersianCalendar();
            var monthProductImagesDirName =
                $"{persianCalendar.GetYear(DateTime.Now)} - {persianCalendar.GetMonth(DateTime.Now)}";
            var rootPath =
                Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot", "img", "Product");
            var productImagesPath =
                Path.Combine(rootPath, "Products", monthProductImagesDirName);
            var isYearMonthProductImagesDirExists =
                Directory.Exists(productImagesPath);
            if (!isYearMonthProductImagesDirExists)
            {
                Directory.CreateDirectory(productImagesPath);
            }

            foreach (var productImageFile in productImagesFiles)
            {
                var productImageFileName =
                    $"{Guid.NewGuid().ToString()} - {productImageFile.FileName}";
                var fullProductImagesPath
                    = Path.Combine(productImagesPath, productImageFileName);
                using (var stream = new FileStream(fullProductImagesPath, FileMode.Create))
                {
                    productImageFile.CopyTo(stream);
                }
                var productImage = new Image()
                {
                    ImagePath = $"Products/{monthProductImagesDirName}/{productImageFileName}",
                    Product = product
                };
                _productRepository.AddImage(productImage);
            }
        }
    }
}
