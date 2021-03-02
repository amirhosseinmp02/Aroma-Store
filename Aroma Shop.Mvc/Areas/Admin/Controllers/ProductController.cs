using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;

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

        [HttpGet("/Admin/Products")]
        public IActionResult Index()
        {
            var model = _productService.GetProducts();
            return View(model);
        }

        [HttpGet("/Admin/Products/Categories")]
        public IActionResult Categories()
        {
            var model = _productService.GetCategories();
            return View(model);
        }

        [HttpGet("/Admin/Products/AddCategory")]
        public IActionResult AddCategory()
        {
            return View();
        }
    }
}
