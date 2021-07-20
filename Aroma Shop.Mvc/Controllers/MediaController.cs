using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Models.MediaModels;

namespace Aroma_Shop.Mvc.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IProductService _productService;

        public MediaController(IMediaService mediaService, IProductService productService)
        {
            _mediaService = mediaService;
            _productService = productService;
        }

        #region ContactUs

        [HttpGet("/Contact-Us")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost("/Contact-Us")]
        [ValidateAntiForgeryToken]
        public IActionResult ContactUs(Message model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    _mediaService.AddMessage(model);

                if (result)
                {
                    ViewData["SuccessMessage"] = "پیام شما با موفقیت ارسال شد.";

                    ModelState.Clear();

                    return View();
                }

                ModelState.AddModelError("", "مشکلی در زمان ارسال پیام رخ داد.");
            }

            return View(model);
        }

        #endregion

        #region AddCommentToProduct

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCommentToProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var productId =
                    Convert.ToInt32(TempData["productId"]);

                var product =
                    _productService.GetProduct(productId);

                product.Comments = product.Comments;

                model.Product = product;

                bool result;

                if (model.ParentCommentId == 0)
                    result = await _mediaService.AddCommentToProduct(model);
                else
                    result = await _mediaService.AddReplyToProductComment(model);

                product.Comments =
                    product.Comments
                        .Where(p => p.IsConfirmed && p.ParentComment == null)
                        .ToList();

                if (result)
                {
                    model.CommentDescription = null;

                    model.ParentCommentId = 0;

                    ViewData["SuccessMessage"] = "دیدگاه / پاسخ شما با موفقیت ثبت و بعد از تایید ادمین نمایش داده خواهد شد";

                    ModelState.Clear(); 

                    return View("/Views/Product/ProductDetails.cshtml", model);
                }

                ModelState.AddModelError("", "مشکلی در زمان ثبت دیدگاه / پاسخ رخ داد.");
            }

            return View("/Views/Product/ProductDetails.cshtml", model);
        }

        #endregion
    }
}
