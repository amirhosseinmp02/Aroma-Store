using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Models.MediaModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Authorize(Policy = "Writer")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMediaService _mediaService;

        public ProductController(IProductService productService, IMediaService mediaService)
        {
            _productService = productService;
            _mediaService = mediaService;
        }

        #region ShowProducts

        [HttpGet("/Admin/Products")]
        public async Task<IActionResult> Index(int pageNumber = 1, string search = null)
        {
            var products =
                await _productService
                    .GetProductsAsync();

            if (!string.IsNullOrEmpty(search))
            {
                products =
                    products
                        .Where(p => p.ProductName.Contains(search) ||
                                    p.Categories.Any(t => t.CategoryName.Contains(search)));

                ViewBag.search = search;
            }

            if (!products.Any())
            {
                ViewBag.isEmpty = true;

                return View();
            }

            var page =
                new Paging<Product>(products, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var productsPage =
                page.QueryResult;

            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;

            return View(productsPage);
        }

        #endregion

        #region AddProduct

        [HttpGet("/Admin/Products/AddProduct")]
        public async Task<IActionResult> AddProduct()
        {
            var productCategories =
                await _productService
                    .GetCategoriesTreeViewForAddAsync();

            var model = new AddEditProductViewModel()
            {
                ProductCategories = productCategories
            };

            return View(model);
        }

        [HttpPost("/Admin/Products/AddProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(AddEditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _productService
                        .AddProductAsync(model);

                if (result)
                {
                    ModelState.Clear();

                    var returnProductCategories =
                        await _productService
                            .GetCategoriesTreeViewForAddAsync();

                    model = new AddEditProductViewModel()
                    {
                        ProductCategories = returnProductCategories
                    };

                    ViewData["SuccessMessage"] = "محصول مورد نظر با موفقیت افزوده شد.";

                    return View(model);
                }

                ModelState.AddModelError("", "مشکلی در زمان افزودن محصول رخ داد.");
            }

            var productCategories =
                await _productService
                    .GetCategoriesTreeViewForAddAsync();

            model.ProductCategories =
                productCategories;

            return View(model);
        }

        #endregion

        #region EditProduct

        [HttpGet("/Admin/Products/EditProduct")]

        public async Task<IActionResult> EditProduct(int productId)
        {
            var product =
                await _productService
                    .GetProductWithDetailsAsync(productId);

            if (product == null)
                return NotFound();

            var productCategories =
                await _productService
                    .GetCategoriesTreeViewForAddAsync();

            foreach (var productCategory in productCategories)
            {
                if (product.Categories
                    .Any(p => p.CategoryId == Convert.ToInt32(productCategory.Value)))
                {
                    productCategory.Selected = true;
                }
            }

            var model = new AddEditProductViewModel()
            {
                ProductName = product.ProductName,
                ProductShortDescription = product.ProductShortDescription,
                ProductDescription = product.ProductDescription,
                IsSimpleProduct = product.IsSimpleProduct,
                ProductPrice = product.ProductPrice,
                ProductQuantityInStock = product.ProductQuantityInStock,
                ProductCategories = productCategories,
                InformationNames = product.Informations.Select(p => p.Name),
                InformationValues = product.Informations.Select(p => p.Value),
                CurrentProductImages = product.Images
            };

            if (!product.IsSimpleProduct)
            {
                model.AttributesNames =
                    product.ProductAttributesNames;

                model.AttributesValues =
                    product.ProductAttributesValues;

                model.ProductVariationsNames =
                    product.ProductVariations
                        .Select(p => string.Join('-', p.ProductVariationValues));

                model.ProductVariationsPrices =
                    product.ProductVariations
                        .Select(p => p?.ProductVariationPrice);

                model.ProductVariationsQuantityInStocks =
                    product.ProductVariations
                        .Select(p => p?.ProductVariationQuantityInStock);
            }

            TempData["productId"] = productId;

            return View(model);
        }

        [HttpPost("/Admin/Products/EditProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(AddEditProductViewModel model)
        {
            model.ProductId =
                Convert.ToInt32(TempData["productId"]);

            if (ModelState.IsValid)
            {
                var result =
                    await _productService
                        .UpdateProductAsync(model);

                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "مشکلی در زمان ویرایش محصول رخ داد.");
            }
            var product =
                await _productService
                    .GetProductWithDetailsAsync(model.ProductId);

            var productCategories =
                await _productService
                    .GetCategoriesTreeViewForAddAsync();

            foreach (var productCategory in productCategories)
            {
                if (product.Categories
                    .Any(p => p.CategoryId == Convert.ToInt32(productCategory.Value)))
                {
                    productCategory.Selected = true;
                }
            }

            model.ProductCategories =
                productCategories;

            model.CurrentProductImages =
                product.Images;

            TempData.Keep("productId");

            return View(model);
        }

        #endregion

        #region DeleteProduct

        [HttpGet("/Admin/Products/DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var result =
                await _productService
                    .DeleteProductByIdAsync(productId);

            if (result)
                return RedirectToAction("Index");

            return NotFound();
        }

        #endregion

        #region ShowCategories

        [HttpGet("/Admin/Products/Categories")]
        public async Task<IActionResult> Categories(int pageNumber = 1, string search = null)
        {
            var categories =
                await _productService
                    .GetCategoriesAsync();

            if (!string.IsNullOrEmpty(search))
            {
                categories =
                    categories
                        .Where(p =>
                    p.CategoryName.Contains(search) ||
                    (Convert.ToBoolean(p.ParentCategory?.CategoryName.Contains(search))));

                ViewBag.search = search;
            }

            if (!categories.Any())
            {
                ViewBag.isEmpty = true;

                return View();
            }

            var page =
                new Paging<Category>(categories, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var categoriesPage =
                page.QueryResult;

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
        public async Task<IActionResult> AddCategory()
        {
            var categoriesTreeView =
                await _productService
                    .GetCategoriesTreeViewForAddAsync();

            var model = new AddEditCategoryViewModel()
            {
                AllCategories = categoriesTreeView
            };

            return View(model);
        }

        [HttpPost("/Admin/Products/AddCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(AddEditCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _productService
                        .AddCategoryAsync(model);

                if (result)
                {
                    ModelState.Clear();

                    model = new AddEditCategoryViewModel()
                    {
                        AllCategories =
                            await _productService
                                .GetCategoriesTreeViewForAddAsync()
                    };

                    ViewData["SuccessMessage"] = "دسته مورد نظر با موفقیت افزوده شد.";

                    return View(model);
                }

                ModelState.AddModelError("", "مشکلی در زمان افزودن دسته رخ داد.");
            }

            model.AllCategories =
                await _productService
                    .GetCategoriesTreeViewForAddAsync();

            return View(model);
        }

        #endregion

        #region EditCategory

        [HttpGet("/Admin/Products/EditCategory")]
        public async Task<IActionResult> EditCategory(int categoryId)
        {
            var category =
                await _productService
                    .GetCategoryAsync(categoryId);

            if (category == null)
                return NotFound();

            var parentCategoryId =
                category.ParentCategory != null ? category.ParentCategory.CategoryId : -1;

            var model = new AddEditCategoryViewModel()
            {
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
                AllCategories = await _productService.GetCategoriesTreeViewForEditAsync(category),
                ParentCategoryId = parentCategoryId
            };

            TempData["categoryId"] = categoryId;

            return View(model);
        }

        [HttpPost("/Admin/Products/EditCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(AddEditCategoryViewModel model)
        {
            model.CategoryId =
                Convert.ToInt32(TempData["categoryId"]);

            if (ModelState.IsValid)
            {
                var result =
                    await _productService
                        .UpdateCategoryAsync(model);

                if (result)
                {
                    return RedirectToAction("Categories");
                }

                ModelState.AddModelError("", "مشکلی در زمان ویرایش دسته رخ داد.");
            }

            var category =
                await _productService
                    .GetCategoryAsync(model.CategoryId);

            var categoryTreeView =
                await _productService
                    .GetCategoriesTreeViewForEditAsync(category);

            model.AllCategories = 
                categoryTreeView;

            model.ParentCategoryId =
                category.ParentCategory.CategoryId;

            TempData.Keep("categoryId");

            return View(model);
        }

        #endregion

        #region DeleteCategory

        [HttpGet("/Admin/Products/DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var result =
                await _productService
                    .DeleteCategoryByIdAsync(categoryId);

            if (result)
                return RedirectToAction("Categories");

            return NotFound();
        }

        #endregion
    }
}
