using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
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
        public IActionResult Discounts(int pageNumber = 1, string search = null)
        {
            IEnumerable<Discount> discounts;

            if (!string.IsNullOrEmpty(search))
            {
                discounts =
                    _productService.GetDiscounts()
                        .Where(p =>
                        p.DiscountCode.Contains(search));

                ViewBag.search = search;
            }
            else
                discounts =
                    _productService.GetDiscounts();

            if (!discounts.Any())
            {
                ViewBag.isEmpty = true;

                return View();
            }

            var page =
                new Paging<Discount>(discounts, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var discountsPage = 
                page.QueryResult;

            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;

            return View(discountsPage);
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

        #region EditDiscount

        [HttpGet("/Admin/Orders/EditDiscount")]
        public IActionResult EditDiscount(int discountId)
        {
            var discount =
                _productService.GetDiscount(discountId);

            if (discount == null)
                return NotFound();

            TempData["discountId"] = discountId;

            return View(discount);
        }

        [HttpPost("/Admin/Orders/EditDiscount")]
        public IActionResult EditDiscount(Discount model)
        {
            model.DiscountId =
                Convert.ToInt32(TempData["discountId"]);

            if (ModelState.IsValid)
            {

            }

            TempData.Keep("discountId");

            return View(model);
        }

        #endregion

        #region MoveDiscountToTrash

        [HttpGet("/Admin/Orders/DeleteDiscount")]
        public IActionResult MoveDiscountToTrash(int discountId)
        {
            var result =
                _productService
                    .MoveDiscountToTrash(discountId);

            if (result)
                return RedirectToAction("Discounts");

            return NotFound();
        }

        #endregion
    }
}
