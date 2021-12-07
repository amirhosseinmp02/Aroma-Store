using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.BlogModels;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.BlogModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IEnumerable<BlogCategory>> GetBlogCategoriesAsync()
        {
            var blogCategories =
                await _blogRepository
                    .GetBlogCategoriesAsync();

            return blogCategories;
        }
        public async Task<IEnumerable<SelectListItem>> GetBlogCategoriesTreeViewForAddAsync()
        {
            var blogCategories =
                await _blogRepository
                    .GetBlogCategoriesAsync();

            List<SelectListItem> items =
                new List<SelectListItem>()
                {
                    new SelectListItem("انتخاب کنید", "-1")
                };

            var parentsCategories =
                blogCategories
                    .Where(p => p.ParentBlogCategory == null);

            foreach (var parentCategory in parentsCategories)
            {
                var item =
                    new SelectListItem
                    (new string('─', 0)
                     + $" {parentCategory.BlogCategoryName}", parentCategory.BlogCategoryId.ToString());

                items
                    .Add(item);

                CategoriesScrolling(parentCategory);
            }

            void CategoriesScrolling(BlogCategory parentCategory, int counter = 0)
            {
                var subCategories =
                    blogCategories
                        .Where(p => p.ParentBlogCategory?.BlogCategoryId == parentCategory.BlogCategoryId);

                foreach (var subCategory in subCategories)
                {
                    ++counter;

                    var item =
                        new SelectListItem
                        (new string('─', counter * 2)
                         + $" {subCategory.BlogCategoryName}", subCategory.BlogCategoryId.ToString());

                    items
                        .Add(item);

                    CategoriesScrolling(subCategory, counter);
                    --counter;
                }
            }

            return items;
        }
        public async Task<IEnumerable<SelectListItem>> GetBlogCategoriesTreeViewForEditAsync(BlogCategory selfCategory)
        {
            var categoriesTreeViewForAdd =
                await GetBlogCategoriesTreeViewForAddAsync();

            var items =
                categoriesTreeViewForAdd.ToList();

            var selfItem =
                items
                    .SingleOrDefault
                    (p => p.Value ==
                          selfCategory.BlogCategoryId.ToString());

            items.Remove(selfItem);

            return items;
        }
        public async Task<BlogCategory> GetBlogCategoryAsync(int blogCategoryId)
        {
            var blogCategory =
                await _blogRepository
                    .GetBlogCategoryAsync(blogCategoryId);

            return blogCategory;
        }
        public async Task<bool> AddBlogCategoryAsync(AddEditBlogCategoryViewModel blogCategoryViewModel)
        {
            try
            {
                var category = new BlogCategory()
                {
                    BlogCategoryName = blogCategoryViewModel.CategoryName,
                    BlogCategoryDescription = blogCategoryViewModel.CategoryDescription
                };

                if (blogCategoryViewModel.ParentCategoryId != -1)
                {
                    var parentCategory =
                        await GetBlogCategoryAsync(blogCategoryViewModel.ParentCategoryId);

                    if (parentCategory != null)
                        category.ParentBlogCategory = parentCategory;
                }

                await _blogRepository
                    .AddBlogCategoryAsync(category);

                await _blogRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }

        }
        public async Task<bool> UpdateBlogCategoryAsync(AddEditBlogCategoryViewModel blogCategoryViewModel)
        {
            try
            {
                var category =
                    await GetBlogCategoryAsync(blogCategoryViewModel.CategoryId);

                category.BlogCategoryName = blogCategoryViewModel.CategoryName;
                category.BlogCategoryDescription = blogCategoryViewModel.CategoryDescription;

                var parentCategory =
                    blogCategoryViewModel.ParentCategoryId == -1
                        ? null
                        : await GetBlogCategoryAsync(blogCategoryViewModel.ParentCategoryId);

                category.ParentBlogCategory = parentCategory;

                _blogRepository
                    .UpdateBlogCategory(category);

                await _blogRepository.
                    SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
        public async Task<bool> DeleteBlogCategoryByIdAsync(int blogCategoryId)
        {
            try
            {
                await DeleteCascadeBlogCategoryByIdAsync(blogCategoryId);

                await _blogRepository
                    .SaveAsync();

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }

        //Utilities Methods

        private async Task<bool> DeleteCascadeBlogCategoryByIdAsync(int blogCategoryId)
        {
            try
            {
                var category =
                    await GetBlogCategoryAsync(blogCategoryId);

                if (category == null)
                    return false;

                var allCategories =
                    await _blogRepository
                        .GetBlogCategoriesAsync();

                var childrenCategories =
                    allCategories
                        .Where(p => p.ParentBlogCategory?.BlogCategoryId == blogCategoryId);

                foreach (var childrenCategory in childrenCategories)
                {
                    DeleteChildrenBlogCategories(childrenCategory);

                    _blogRepository
                        .DeleteBlogCategory(childrenCategory);
                }

                void DeleteChildrenBlogCategories(BlogCategory childrenCategory)
                {
                    var subCategories =
                        allCategories
                            .Where(p => p.ParentBlogCategory?.BlogCategoryId == childrenCategory.BlogCategoryId);

                    foreach (var subCategory in subCategories)
                    {
                        DeleteChildrenBlogCategories(subCategory);

                        _blogRepository
                            .DeleteBlogCategory(subCategory);
                    }
                }

                _blogRepository
                    .DeleteBlogCategory(category);

                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }
    }
}
