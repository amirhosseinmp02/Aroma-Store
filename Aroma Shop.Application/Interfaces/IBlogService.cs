using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Application.ViewModels.BlogModels;
using Aroma_Shop.Domain.Models.BlogModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogCategory>> GetBlogCategoriesAsync();
        Task<IEnumerable<SelectListItem>> GetBlogCategoriesTreeViewForAddAsync();
        Task<IEnumerable<SelectListItem>> GetBlogCategoriesTreeViewForEditAsync(BlogCategory selfCategory);
        public Task<BlogCategory> GetBlogCategoryAsync(int blogCategoryId);
        public Task<bool> AddBlogCategoryAsync(AddEditBlogCategoryViewModel blogCategoryViewModel);
        public Task<bool> UpdateBlogCategoryAsync(AddEditBlogCategoryViewModel blogCategoryViewModel);
        public Task<bool> DeleteBlogCategoryByIdAsync(int blogCategoryId);
    }
}
