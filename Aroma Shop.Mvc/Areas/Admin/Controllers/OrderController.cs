using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Authorize(Policy = "Writer")]
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IProductService _productService;

        public OrderController(IProductService productService)
        {
            _productService = productService;
        }

        #region ShowOrders

        [HttpGet("/Admin/Orders")]
        public async Task<IActionResult> Index(int pageNumber = 1, string search = null)
        {
            var ordersViewModels =
                await _productService
                    .GetOrdersListViewAsync();

            if (!string.IsNullOrEmpty(search))
            {
                ordersViewModels =
                    ordersViewModels
                        .Where(p => p.OrderId.ToString() == search
                                                || p.OrderName
                                                    .Contains(search) && p.NotEmpty);
            }
            else
                ordersViewModels =
                    ordersViewModels
                    .Where(p => p.NotEmpty);

            if (!ordersViewModels.Any())
            {
                ViewData["isEmpty"] = true;

                return View();
            }

            var page =
                new Paging<OrdersViewModel>(ordersViewModels, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var ordersViewModelsPage =
                page.QueryResult;

            ViewData["pageNumber"] = pageNumber;
            ViewData["firstPage"] = page.FirstPage;
            ViewData["lastPage"] = page.LastPage;
            ViewData["prevPage"] = page.PreviousPage;
            ViewData["nextPage"] = page.NextPage;
            ViewData["search"] = search;
            ViewData["isEmpty"] = false;

            return View(ordersViewModels);
        }

        #endregion

        #region EditOrder

        [HttpGet("/Admin/Orders/EditOrder")]
        public async Task<IActionResult> EditOrder(int orderId)
        {
            var order =
                await _productService
                    .GetOrderForEditAsync(orderId);

            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost("/Admin/Orders/EditOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrder(Order model)
        {
            ModelState.Remove("OwnerUser");

            if (ModelState.IsValid)
            {
                var result =
                    await _productService
                        .UpdateOrderAsync(model);

                if (result)
                    return RedirectToAction("Index");
            }

            return NotFound();
        }

        #endregion

        #region DeleteOrder

        [HttpGet("/Admin/Orders/DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var result =
                await _productService
                    .DeleteOrderByIdAsync(orderId);

            if (result)
                return RedirectToAction("Index");

            return NotFound();
        }

        #endregion

        #region ShowDiscounts

        [HttpGet("/Admin/Orders/Discounts")]
        public async Task<IActionResult> Discounts(int pageNumber = 1, string search = null)
        {
            var discounts =
                await _productService
                    .GetDiscountsAsync();

            if (!string.IsNullOrEmpty(search))
            {
                discounts = 
                    discounts
                        .Where(p =>
                        p.DiscountCode.Contains(search) && !p.IsTrash);
            }
            else
                discounts =
                    discounts
                        .Where(p => !p.IsTrash);

            if (!discounts.Any())
            {
                ViewData["isEmpty"] = true;

                return View();
            }

            var page =
                new Paging<Discount>(discounts, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var discountsPage =
                page.QueryResult;

            ViewData["pageNumber"] = pageNumber;
            ViewData["firstPage"] = page.FirstPage;
            ViewData["lastPage"] = page.LastPage;
            ViewData["prevPage"] = page.PreviousPage;
            ViewData["nextPage"] = page.NextPage;
            ViewData["search"] = search;
            ViewData["isEmpty"] = false;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDiscount(Discount model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _productService
                        .AddDiscountAsync(model);

                if (result == AddUpdateDiscountResult.Successful)
                {
                    ModelState.Clear();

                    ViewData["SuccessMessage"] = "کد تخفیف مورد نظر با موفقیت افزوده شد.";

                    return View();
                }

                else if (result == AddUpdateDiscountResult.DiscountCodeExist)
                    ModelState.AddModelError("", "این کد تخفیف در حال حاضر موجود است");

                else
                    ModelState.AddModelError("", "مشکلی در زمان افزودن کد تخفیف رخ داد.");
            }

            return View(model);
        }

        #endregion

        #region EditDiscount

        [HttpGet("/Admin/Orders/EditDiscount")]
        public async Task<IActionResult> EditDiscount(int discountId)
        {
            var discount =
                await _productService
                    .GetDiscountAsync(discountId);

            if (discount == null)
                return NotFound();

            TempData["discountId"] = discountId;

            return View(discount);
        }

        [HttpPost("/Admin/Orders/EditDiscount")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDiscount(Discount model)
        {
            model.DiscountId =
                Convert.ToInt32(TempData["discountId"]);

            if (ModelState.IsValid)
            {
                var result =
                    await _productService
                        .UpdateDiscountAsync(model);

                if (result == AddUpdateDiscountResult.Successful)
                    return RedirectToAction("Discounts");

                else if (result == AddUpdateDiscountResult.DiscountCodeExist)
                    ModelState.AddModelError("", "این کد تخفیف در حال حاضر موجود است");

                else
                    ModelState.AddModelError("", "مشکلی در زمان افزودن کد تخفیف رخ داد.");
            }

            TempData.Keep("discountId");

            return View(model);
        }

        #endregion

        #region MoveDiscountToTrash

        [HttpGet("/Admin/Orders/DeleteDiscount")]
        public async Task<IActionResult> MoveDiscountToTrash(int discountId)
        {
            var result =
                await _productService
                    .MoveDiscountToTrashAsync(discountId);

            if (result)
                return RedirectToAction("Discounts");

            return NotFound();
        }

        #endregion
    }
}
