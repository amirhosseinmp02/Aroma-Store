using Microsoft.AspNetCore.Mvc;
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
            var model = new AddCategoryViewModel()
            {
                AllCategories = _productService.GetCategoriesTreeView(_productService.GetCategories())
            };
            return View(model);
        }

        [HttpPost("/Admin/Products/AddCategory")]
        public IActionResult AddCategory(AddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }

        #endregion

    }
}
