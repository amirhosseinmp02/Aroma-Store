using System;
using System.Collections.Generic;
using System.Linq;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.ProductModels;
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

        public IEnumerable<SelectListItem> GetCategoriesTreeView(IEnumerable<Category> categories)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var parentsCategories =
                categories.Where(p => p.ParentCategory == null);
            int count = 0;
            ParentsCategoriesScrolling(parentsCategories);
            void ParentsCategoriesScrolling(IEnumerable<Category> parents)
            {
                foreach (var parent in parents)
                {
                    items.Add(new SelectListItem(new string('─', count*2) + $" {parent.CategoryName}", parent.CategoryId.ToString()));
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
                    items.Add(new SelectListItem(new string('─', counter*2) + $" {child.CategoryName}", child.CategoryId.ToString()));
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

        public bool AddCategory(AddCategoryViewModel categoryViewModel)
        {
            try
            {
                var category = new Category()
                {
                    CategoryName = categoryViewModel.CategoryName,
                    CategoryDescription = categoryViewModel.CategoryDescription
                };
                var parentCategory =
                    _productRepository
                        .GetCategory(categoryViewModel.ParentCategoryId);
                category.ParentCategory = parentCategory;
                _productRepository.AddCategory(category);
                _productRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
