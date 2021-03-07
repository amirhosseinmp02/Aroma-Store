using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #region ShowProducts

        [HttpGet("/Admin/Products")]
        public IActionResult Index(int pageNumber = 1, string search = null)
        {
            IEnumerable<Product> products;
            if (!string.IsNullOrEmpty(search))
            {
                products = _productService.GetProducts()
                    .Where(p => p.ProductName.Contains(search)
                                || p.Categories
                                    .Contains(new Category(){CategoryName = "search"}));
                ViewBag.search = search;
            }
            else
                products = _productService.GetProducts();
            if (products.Count() == 0)
            {
                ViewBag.isEmpty = true;
                return View();
            }
            var page = new Paging<Product>(products, 11, pageNumber);
            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();
            var categoriesPage = page.QueryResult;
            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;
            return View(categoriesPage);
        }

        #endregion

        #region AddProduct

        public IActionResult AddProduct()
        {
            return View();
        }

        #endregion

        #region ShowCategories

        [HttpGet("/Admin/Products/Categories")]
        public IActionResult Categories(int pageNumber = 1, string search = null)
        {
            IEnumerable<Category> categories;
            if (!string.IsNullOrEmpty(search))
            {
                categories = _productService.GetCategories().Where(p =>
                    p.CategoryName.Contains(search) ||
                    (Convert.ToBoolean(p.ParentCategory?.CategoryName.Contains(search))));
                ViewBag.search = search;
            }
            else
                categories = _productService.GetCategories();
            if (categories.Count() == 0)
            {
                ViewBag.isEmpty = true;
                return View();
            }
            var page = new Paging<Category>(categories, 11, pageNumber);
            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();
            var categoriesPage = page.QueryResult;
            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;
            return View(categoriesPage);
        }

        #endregion

        #region AddCategory

        [HttpGet("/Admin/Products/AddCategory")]
        public IActionResult AddCategory()
        {
            var categories = _productService.GetCategories();
            var model = new AddEditCategoryViewModel()
            {
                AllCategories = _productService.GetCategoriesTreeView()
            };
            return View(model);
        }

        [HttpPost("/Admin/Products/AddCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(AddEditCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _productService.AddCategory(model);
                if (result)
                {
                    ModelState.Clear();
                    var newCategories = _productService.GetCategories();
                    model = new AddEditCategoryViewModel()
                    {
                        AllCategories = _productService.GetCategoriesTreeView()
                    };
                    ViewData["SuccessMessage"] = "دسته مورد نظر با موفقیت افزوده شد.";
                    return View(model);
                }
                ModelState.AddModelError("", "مشکلی در زمان افزودن دسته رخ داد.");
            }
            var categories = _productService.GetCategories();
            model.AllCategories =
                _productService.GetCategoriesTreeView();
            return View(model);
        }

        #endregion

        #region EditCategory

        [HttpGet("/Admin/Products/EditCategory")]
        public IActionResult EditCategory(int categoryId)
        {
            var category = _productService.GetCategory(categoryId);
            if (category == null)
                return NotFound();
            var model = new AddEditCategoryViewModel()
            {
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
                AllCategories = _productService.GetCategoriesTreeViewForEdit(category),
                ParentCategoryId = category.ParentCategory?.CategoryId
            };
            TempData["categoryId"] = categoryId;
            return View(model);
        }

        [HttpPost("/Admin/Products/EditCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(AddEditCategoryViewModel model)
        {
            model.CategoryId = Convert.ToInt32(TempData["categoryId"]);
            var category = _productService.GetCategory(model.CategoryId);
            if (category == null)
                return NotFound();
            var categoryTreeView =
                _productService.GetCategoriesTreeViewForEdit(category);
            if (ModelState.IsValid)
            {
                var result = _productService.UpdateCategory(model);
                if (result)
                {
                    return RedirectToAction("Categories");
                }
                ModelState.AddModelError("", "مشکلی در زمان ویرایش دسته رخ داد.");
            }

            model = new AddEditCategoryViewModel()
            {
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
                AllCategories = categoryTreeView,
                ParentCategoryId = category.ParentCategory?.CategoryId
            };

            return View(model);
        }

        #endregion

        #region DeleteCategory

        [HttpGet("/Admin/Products/DeleteCategory")]
        public IActionResult DeleteCategory(int categoryId)
        {
            var result = _productService.DeleteCategory(categoryId);
            if (result)
                return RedirectToAction("Categories");
            return NotFound();
        }

        #endregion

    }
}
