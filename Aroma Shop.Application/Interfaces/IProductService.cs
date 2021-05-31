using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
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
        bool UpdateProduct(AddEditProductViewModel productViewModel);
        bool DeleteProductById(int productId);
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        bool AddCategory(AddEditCategoryViewModel categoryViewModel);
        bool UpdateCategory(AddEditCategoryViewModel categoryViewModel);
        bool DeleteCategoryById(int categoryId);
        IEnumerable<SelectListItem> GetCategoriesTreeView();
        IEnumerable<SelectListItem> GetCategoriesTreeViewForEdit(Category selfCategory);
        IEnumerable<Comment> GetComments();
        int GetUnreadCommentsCount();
        Task<bool> AddReplyToCommentByAdmin(int commentId, string newCommentReplyDescription);
        Task<bool> AddCommentToProduct(ProductViewModel productViewModel);
        Task<bool> AddReplyToProductComment(ProductViewModel productViewModel);
        Comment GetComment(int commentId);
        bool UpdateComment(Comment comment);
        bool SetCommentAsRead(Comment comment);
        bool DeleteCommentById(int commentId);
        bool ConfirmComment(int commentId);
        bool RejectComment(int commentId);
    }
}
