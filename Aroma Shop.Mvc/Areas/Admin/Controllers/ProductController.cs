﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
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
        public IActionResult Index()
        {
            var model = _productService.GetProducts();
            return View(model);
        }

        #endregion

        #region ShowCategories

        [HttpGet("/Admin/Products/Categories")]
        public IActionResult Categories()
        {
            var model = _productService.GetCategories();
            return View(model);
        }

        #endregion

        #region AddCategory

        [HttpGet("/Admin/Products/AddCategory")]
        public IActionResult AddCategory()
        {
            var categories = _productService.GetCategories();
            var model = new AddEditCategoryViewModel()
            {
                AllCategories = _productService.GetCategoriesTreeView(categories)
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
                        AllCategories = _productService.GetCategoriesTreeView(newCategories)
                    };
                    ViewData["SuccessMessage"] = "دسته مورد نظر با موفقیت افزوده شد.";
                    return View(model);
                }
                ModelState.AddModelError("", "مشکلی در زمان افزودن دسته رخ داد.");
            }
            var categories = _productService.GetCategories();
            model.AllCategories =
                _productService.GetCategoriesTreeView(categories);
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
            var categories = _productService.GetCategories();
            var model = new AddEditCategoryViewModel()
            {
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
                AllCategories = _productService.GetCategoriesTreeView(categories),
                ParentCategoryId = category.ParentCategory.CategoryId
            };
            model.AllCategories
                .SingleOrDefault
                    (p => p.Value == model.ParentCategoryId.ToString())
                .Selected = true;
            return View(model);
        }

        [HttpPost("/Admin/Products/EditCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(AddEditCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                
            }

            return View();
        }

        #endregion

        #region DeleteCategory

        [HttpGet("/Admin/Products/DeleteCategory")]
        public IActionResult DeleteCategory(int categoryId)
        {
            var result = _productService.DeleteCategory(categoryId);
            if(result)
                return RedirectToAction("Categories");
            return NotFound();
        }

        #endregion
    }
}
