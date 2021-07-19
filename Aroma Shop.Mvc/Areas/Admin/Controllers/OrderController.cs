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

        #region ShowDiscounts

        [HttpGet("/Admin/Orders/Discounts")]
        public IActionResult Discounts()
        {
            return View();
        }

        #endregion

        #region AddDiscount

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
                    _productService.AddDiscount(model);

                if (result)
                {
                    ModelState.Clear();

                    ViewData["SuccessMessage"] = "کد تخفیف مورد نظر با موفقیت افزوده شد.";

                    return View();
                }

                ModelState.AddModelError("", "مشکلی در زمان افزودن کد تخفیف رخ داد.");
            }

            return View(model);
        }

        #endregion
    }
}
