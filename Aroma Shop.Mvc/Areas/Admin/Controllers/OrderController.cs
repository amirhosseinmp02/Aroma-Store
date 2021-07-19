using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IProductService _productService;

        public OrderController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("/Admin/Orders/AddDiscount")]
        public IActionResult AddDiscount()
        {
            return View();
        }

        [HttpPost("/Admin/Orders/AddDiscount")]
        public IActionResult AddDiscount(Discount model)
        {
            if (ModelState.IsValid)
            {
                var result = 
                    _productService.AddDiscount()
            }
        }
    }
}
