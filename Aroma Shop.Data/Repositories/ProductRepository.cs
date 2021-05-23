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
                .Include(p => p.Images)
                .Include(p=>p.Comments)
                .ThenInclude(p=>p.User)
                .Include(p=>p.Comments)
                .ThenInclude(p=>p.Replies)
                .ThenInclude(p=>p.User)
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


        public void AddProductInformation(ProductInformation productInformation)
        {
            _context.Add(productInformation);
        }

        public void DeleteProductInformation(ProductInformation productInformation)
        {
            _context.Remove(productInformation);
        }

        public void UpdateProduct(Product product)
        {
            _context.Update(product);
        }

        public void DeleteProduct(Product product)
        {
            _context.Remove(product);
        }

        public void AddCategory(Category category)
        {
            _context.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            _context.Update(category);
        }

        public void DeleteCategory(Category category)
        {
            _context.Remove(category);
        }

        public void DeleteCategoryById(int categoryId)
        {
            var category = GetCategory(categoryId);

            _context.Remove(category);
        }

        public Image GetImage(int imageId)
        {
            var image = _context.Images.Find(imageId);
            return image;
        }

        public void AddImage(Image image)
        {
            _context.Add(image);
        }

        public void DeleteImage(Image image)
        {
            _context.Remove(image);
        }

        public Comment GetComment(int commentId)
        {
            var comment = 
                _context.Comments.Find(commentId);

            return comment;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
