using System;
using System.Collections.Generic;
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
            var productCategories = new List<Category>();
            foreach (var productCategoryId in productViewModel.ProductCategoriesId)
            {
                productCategories.Add(GetCategory(productCategoryId));
            }

            throw new Exception();

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

        public IEnumerable<Image> AddProductImages(IEnumerable<IFormFile> productImages)
        {
            string productImagesPath = Path.Combine(Directory.GetCurrentDirectory());
            throw new Exception();
        }
    }
}
