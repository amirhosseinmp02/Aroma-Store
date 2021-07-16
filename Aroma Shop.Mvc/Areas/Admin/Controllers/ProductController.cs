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
        public IActionResult Index(int pageNumber = 1, string search = null)
        {
            IEnumerable<Product> products;

            if (!string.IsNullOrEmpty(search))
            {
                products = _productService.GetProducts()
                    .Where(p => p.ProductName.Contains(search)
                                || p.Categories
                                    .Contains(new Category() { CategoryName = search }));

                ViewBag.search = search;
            }
            else
                products = _productService.GetProducts();

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
        public IActionResult AddProduct()
        {
            var productCategories =
                _productService.GetCategoriesTreeView().Skip(1);

            var model = new AddEditProductViewModel()
            {
                ProductCategories = productCategories
            };

            return View(model);
        }

        [HttpPost("/Admin/Products/AddProduct")]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(AddEditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    _productService.AddProduct(model);

                if (result)
                {
                    ModelState.Clear();

                    var returnProductCategories =
                        _productService.GetCategoriesTreeView().Skip(1);

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
                _productService.GetCategoriesTreeView();

            model.ProductCategories = productCategories;

            return View(model);
        }

        #endregion

        #region EditProduct

        [HttpGet("/Admin/Products/EditProduct")]

        public IActionResult EditProduct(int productId)
        {
            var product =
                _productService.GetProduct(productId);

            if (product == null)
                return NotFound();

            var productCategories =
                _productService.GetCategoriesTreeView().Skip(1);

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
                    product.ProductAttributes
                        .Select(p => p.ProductAttributeName);

                for (int i = 0; i < product.ProductAttributes.Count; i++)
                {
                    var stringedProductAttributeValue = "";

                    foreach (var productAttributeValue in product.ProductAttributes.ElementAtOrDefault(i).ProductAttributeValues)
                    {
                        if (!string.IsNullOrEmpty(stringedProductAttributeValue))
                        {
                            stringedProductAttributeValue += $",{productAttributeValue.AttributeValue}";
                        }
                        else
                        {
                            stringedProductAttributeValue = productAttributeValue.AttributeValue;
                        }
                    }

                    model.AttributesValues.Add(stringedProductAttributeValue);
                }

                model.ProductVariationsNames =
                    product.ProductVariations
                        .Select(p => p.ProductVariationValue);

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
        public IActionResult EditProduct(AddEditProductViewModel model)
        {
            model.ProductId =
                Convert.ToInt32(TempData["productId"]);

            if (ModelState.IsValid)
            {
                var result =
                    _productService.UpdateProduct(model);

                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "مشکلی در زمان ویرایش محصول رخ داد.");
            }
            var product =
                _productService.GetProduct(model.ProductId);

            var productCategories =
                _productService.GetCategoriesTreeView().Skip(1);

            foreach (var productCategory in productCategories)
            {
                if (product.Categories
                    .Any(p => p.CategoryId == Convert.ToInt32(productCategory.Value)))
                {
                    productCategory.Selected = true;
                }
            }

            model.ProductCategories = productCategories;

            model.CurrentProductImages = product.Images;

            TempData.Keep("productId");

            return View(model);
        }

        #endregion

        #region DeleteProduct

        [HttpGet("/Admin/Products/DeleteProduct")]
        public IActionResult DeleteProduct(int productId)
        {
            var result =
                _productService.DeleteProductById(productId);

            if (result)
                return RedirectToAction("Index");

            return NotFound();
        }

        #endregion

        #region ShowCategories

        [HttpGet("/Admin/Products/Categories")]
        public IActionResult Categories(int pageNumber = 1, string search = null)
        {
            IEnumerable<Category> categories;

            if (!string.IsNullOrEmpty(search))
            {
                categories =
                    _productService.GetCategories().Where(p =>
                    p.CategoryName.Contains(search) ||
                    (Convert.ToBoolean(p.ParentCategory?.CategoryName.Contains(search))));

                ViewBag.search = search;
            }
            else
                categories =
                    _productService.GetCategories();

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
        public IActionResult AddCategory()
        {
            var categories =
                _productService.GetCategories();

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
                var result =
                    _productService.AddCategory(model);

                if (result)
                {
                    ModelState.Clear();

                    model = new AddEditCategoryViewModel()
                    {
                        AllCategories =
                            _productService.GetCategoriesTreeView()
                    };

                    ViewData["SuccessMessage"] = "دسته مورد نظر با موفقیت افزوده شد.";

                    return View(model);
                }

                ModelState.AddModelError("", "مشکلی در زمان افزودن دسته رخ داد.");
            }

            model.AllCategories =
                _productService.GetCategoriesTreeView();

            return View(model);
        }

        #endregion

        #region EditCategory

        [HttpGet("/Admin/Products/EditCategory")]
        public IActionResult EditCategory(int categoryId)
        {
            var category =
                _productService.GetCategory(categoryId);
            if (category == null)
                return NotFound();

            var parentCategoryId =
                category.ParentCategory != null ? category.ParentCategory.CategoryId : -1;

            var model = new AddEditCategoryViewModel()
            {
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
                AllCategories = _productService.GetCategoriesTreeViewForEdit(category),
                ParentCategoryId = parentCategoryId
            };

            TempData["categoryId"] = categoryId;

            return View(model);
        }

        [HttpPost("/Admin/Products/EditCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(AddEditCategoryViewModel model)
        {
            model.CategoryId =
                Convert.ToInt32(TempData["categoryId"]);

            if (ModelState.IsValid)
            {
                var result =
                    _productService.UpdateCategory(model);

                if (result)
                {
                    return RedirectToAction("Categories");
                }

                ModelState.AddModelError("", "مشکلی در زمان ویرایش دسته رخ داد.");
            }

            var category =
                _productService.GetCategory(model.CategoryId);

            var categoryTreeView =
                _productService.GetCategoriesTreeViewForEdit(category);

            model.AllCategories = categoryTreeView;
            model.ParentCategoryId =
                category.ParentCategory.CategoryId;

            TempData.Keep("categoryId");

            return View(model);
        }

        #endregion

        #region DeleteCategory

        [HttpGet("/Admin/Products/DeleteCategory")]
        public IActionResult DeleteCategory(int categoryId)
        {
            var result =
                _productService.DeleteCategoryById(categoryId);

            if (result)
                return RedirectToAction("Categories");

            return NotFound();
        }

        #endregion
    }
}
