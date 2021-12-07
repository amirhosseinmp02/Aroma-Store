using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.BlogModels;
using Aroma_Shop.Domain.Models.BlogModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Authorization;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Authorize(Policy = "Writer")]
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        #region AddBlog

        [HttpGet("/Admin/Blogs/AddBlog")]
        public async Task<IActionResult> AddBlog()
        {
            return View();
        }

        #endregion

        #region ShowBlogCategories

        [HttpGet("/Admin/Blogs/Categories")]
        public async Task<IActionResult> Categories(int pageNumber = 1, string search = null)
        {
            var blogCategories =
                await _blogService
                    .GetBlogCategoriesAsync();

            if (!string.IsNullOrEmpty(search))
            {
                blogCategories =
                    blogCategories
                        .Where(p =>
                            p.BlogCategoryName.Contains(search) ||
                            (Convert.ToBoolean(p.ParentBlogCategory?.BlogCategoryName.Contains(search))));
            }

            if (!blogCategories.Any())
            {
                ViewData["isEmpty"] = true;

                return View();
            }

            var page =
                new Paging<BlogCategory>(blogCategories, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var blogCategoriesPage =
                page.QueryResult;

            ViewData["pageNumber"] = pageNumber;
            ViewData["firstPage"] = page.FirstPage;
            ViewData["lastPage"] = page.LastPage;
            ViewData["prevPage"] = page.PreviousPage;
            ViewData["nextPage"] = page.NextPage;
            ViewData["search"] = search;
            ViewData["isEmpty"] = false;

            return View(blogCategoriesPage);
        }

        #endregion

        #region AddCategory

        [HttpGet("/Admin/Blogs/AddCategory")]
        public async Task<IActionResult> AddCategory()
        {
            var blogCategoriesTreeView =
                await _blogService
                    .GetBlogCategoriesTreeViewForAddAsync();

            var addEditBlogCategoryViewModel = new AddEditBlogCategoryViewModel()
            {
                AllCategories = blogCategoriesTreeView
            };

            return View(addEditBlogCategoryViewModel);
        }

        [HttpPost("/Admin/Blogs/AddCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(AddEditBlogCategoryViewModel model)
        {
            

            if (ModelState.IsValid)
            {
                var result =
                    await _blogService
                        .AddBlogCategoryAsync(model);

                if (result)
                {
                    ModelState.Clear();

                    var blogCategoriesTreeView =
                        await _blogService
                            .GetBlogCategoriesTreeViewForAddAsync();

                    model = new AddEditBlogCategoryViewModel()
                    {
                        AllCategories = blogCategoriesTreeView
                    };

                    ViewData["SuccessMessage"] = "دسته مورد نظر با موفقیت افزوده شد.";

                    return View(model);
                }

                ModelState.AddModelError("", "مشکلی در زمان افزودن دسته رخ داد.");
            }

            model.AllCategories =
                await _blogService
                    .GetBlogCategoriesTreeViewForAddAsync();

            return View(model);
        }

        #endregion

        #region EditCategory

        [HttpGet("/Admin/Blogs/EditCategory")]
        public async Task<IActionResult> EditCategory(int blogCategoryId)
        {
            var blogCategory =
                await _blogService
                    .GetBlogCategoryAsync(blogCategoryId);

            if (blogCategory == null)
                return NotFound();

            var parentCategoryId =
                blogCategory.ParentBlogCategory != null ? blogCategory.ParentBlogCategory.BlogCategoryId : -1;

            var model = new AddEditBlogCategoryViewModel()
            {
                CategoryName = blogCategory.BlogCategoryName,
                CategoryDescription = blogCategory.BlogCategoryDescription,
                AllCategories = await _blogService.GetBlogCategoriesTreeViewForEditAsync(blogCategory),
                ParentCategoryId = parentCategoryId
            };

            TempData["blogCategoryId"] = blogCategoryId;

            return View(model);
        }

        [HttpPost("/Admin/Blogs/EditCategory")]
        public async Task<IActionResult> EditCategory(AddEditBlogCategoryViewModel model)
        {
            model.CategoryId =
                Convert.ToInt32(TempData["blogCategoryId"]);

            if (ModelState.IsValid)
            {
                var result =
                    await _blogService
                        .UpdateBlogCategoryAsync(model);

                if (result)
                {
                    return RedirectToAction("Categories");
                }

                ModelState.AddModelError("", "مشکلی در زمان ویرایش دسته رخ داد.");
            }

            var blogCategory =
                await _blogService
                    .GetBlogCategoryAsync(model.CategoryId);

            var categoryTreeView =
                await _blogService
                    .GetBlogCategoriesTreeViewForEditAsync(blogCategory);

            model.AllCategories =
                categoryTreeView;

            model.ParentCategoryId =
                blogCategory.ParentBlogCategory.BlogCategoryId;

            TempData.Keep("blogCategoryId");

            return View(model);
        }

        #endregion

        #region DeleteCategory

        [HttpGet("/Admin/Blogs/DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int blogCategoryId)
        {
            var result =
                await _blogService
                    .DeleteBlogCategoryByIdAsync(blogCategoryId);

            if (result)
                return RedirectToAction("Categories");

            return NotFound();
        }

        #endregion
    }
}
