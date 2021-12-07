using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.BlogModels;
using Microsoft.EntityFrameworkCore;

namespace Aroma_Shop.Data.Repositories
{
    public class BlogRepository : IBlogRepository
    {

        private readonly AppDbContext _context;

        public BlogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Blog>> GetBlogsAsync()
        {
            var blogs =
                await _context
                    .Blogs
                    .Include(p => p.BlogImage)
                    .ToListAsync();

            return blogs;
        }
        public async Task<IEnumerable<BlogCategory>> GetBlogCategoriesAsync()
        {
            var blogCategories =
                await _context
                    .BlogsCategories
                    .Include(p => p.ParentBlogCategory)
                    .ToListAsync();

            return blogCategories;
        }
        public async Task<Blog> GetBlogAsync(int blogId)
        {
            var blog =
                await _context
                    .Blogs
                    .FindAsync(blogId);

            return blog;
        }
        public async Task<BlogCategory> GetBlogCategoryAsync(int blogCategoryId)
        {
            var blogCategory =
                await _context
                    .BlogsCategories
                    .Include(p=>p.ParentBlogCategory)
                    .SingleOrDefaultAsync(p=>p.BlogCategoryId==blogCategoryId);

            return blogCategory;
        }
        public async Task AddBlogAsync(Blog blog)
        {
            await _context
                .AddAsync(blog);
        }
        public async Task AddBlogCategoryAsync(BlogCategory blogCategory)
        {
            await _context
                    .AddAsync(blogCategory);
        }
        public void DeleteBlog(Blog blog)
        {
            _context
                .Remove(blog);
        }
        public void DeleteBlogCategory(BlogCategory blogCategory)
        {
            _context
                .Remove(blogCategory);
        }
        public void UpdateBlog(Blog blog)
        {
            _context
                .Update(blog);
        }
        public void UpdateBlogCategory(BlogCategory blogCategory)
        {
            _context
                .Update(blogCategory);
        }
        public async Task SaveAsync()
        {
            await 
                _context
                    .SaveChangesAsync();
        }
    }
}
