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

        public bool AddCategory(Category category)
        {
            try
            {
                _context.Add(category);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
