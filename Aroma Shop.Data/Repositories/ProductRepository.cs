using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.EntityFrameworkCore;

namespace Aroma_Shop.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetProducts()
        {
            var products = _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Images);
            return products;
        }

        public Product GetProduct(int productId)
        {
            var product = _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Informations)
                .Include(p => p.Comments)
                .Include(p => p.Images)
                .SingleOrDefault(p => p.ProductId == productId);
            return product;
        }

        public IEnumerable<Category> GetCategories()
        {
            var categories = _context.Categories
                .Include(p => p.ParentCategory)
                .Include(p => p.ChildrenCategories)
                .ThenInclude(p=>p.ChildrenCategories).ThenInclude(p=>p.ChildrenCategories)
                .ThenInclude(p=>p.ChildrenCategories).ThenInclude(p=>p.ChildrenCategories);
            return categories;
        }

        public Category GetCategory(int categoryId)
        {
            var category = _context.Categories
                .Include(p => p.ParentCategory)
                .Include(p => p.ChildrenCategories)
                .SingleOrDefault(p => p.CategoryId == categoryId);
            return category;
        }

        public void AddProduct(Product product)
        {
            _context.Add(product);
        }


        public void AddProductInformations(ProductInformation productInformation)
        {
            _context.Add(productInformation);
        }

        public void DeleteProductInformations(Product product)
        {
            product.Informations.Clear(); 
        }

        public void UpdateProduct(Product product)
        {
            _context.Update(product);
        }

        public void AddCategory(Category category)
        {
            _context.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            _context.Update(category);
        }

        public void DeleteCategoryById(int categoryId)
        {
            var category = GetCategory(categoryId);
            _context.Remove(category);
            if (category.ChildrenCategories.Count != 0)
            {
                ChildrenCategoriesScrolling(category.ChildrenCategories);
                void ChildrenCategoriesScrolling(IEnumerable<Category> children)
                {
                    foreach (var child in children)
                    {
                        _context.Remove(child);
                        var temp = GetCategory(child.CategoryId);
                        if(temp.ChildrenCategories.Count != 0)
                            ChildrenCategoriesScrolling(temp.ChildrenCategories);
                    }
                }
            }
        }

        public IEnumerable<Image> GetImages()
        {
            var images = _context.Images;
            return images;
        }

        public void AddImage(Image image)
        {
            _context.Add(image);
        }

        public void DeleteImage(Image image)
        {
            _context.Remove(image);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
