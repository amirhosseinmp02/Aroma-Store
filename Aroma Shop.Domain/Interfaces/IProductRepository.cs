﻿using System.Collections.Generic;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int productId);
        void AddProduct(Product product);
        void AddProductInformation(ProductInformation productInformation);
        void DeleteProductInformation(ProductInformation productInformation);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        void AddCategory(Category category);
        void DeleteCategory(Category category);
        void DeleteCategoryById(int categoryId);
        void UpdateCategory(Category category);
        Image GetImage(int imageId);
        void AddImage(Image image);
        void DeleteImage(Image image);
        Comment GetComment(int commentId);
        IEnumerable<Comment> GetComments();
        void DeleteComment(Comment comment);
        void DeleteCommentById(int commentId);
        void Save();
    }
}
