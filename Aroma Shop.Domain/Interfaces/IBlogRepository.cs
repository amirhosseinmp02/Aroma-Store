using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Domain.Models.BlogModels;

namespace Aroma_Shop.Domain.Interfaces
{  
    public interface IBlogRepository : IGeneralRepository
    {
        Task<IEnumerable<Blog>> GetBlogsAsync();
        Task<IEnumerable<BlogCategory>> GetBlogCategoriesAsync();
        Task<Blog> GetBlogAsync(int blogId);
        Task<BlogCategory> GetBlogCategoryAsync(int blogCategoryId);
        Task AddBlogAsync(Blog blog);
        Task AddBlogCategoryAsync(BlogCategory blogCategory);
        void DeleteBlog(Blog blog);
        void DeleteBlogCategory(BlogCategory blogCategory);
        void UpdateBlog(Blog blog);
        void UpdateBlogCategory(BlogCategory blogCategory);
    }
}
