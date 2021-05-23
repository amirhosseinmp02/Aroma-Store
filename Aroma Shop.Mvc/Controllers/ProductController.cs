using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.Product;

namespace Aroma_Shop.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #region ProductDetails

        [HttpGet("/Products/{productId}")]
        public IActionResult ProductDetails(int productId)
        {
            var product = _productService.GetProduct(productId);
            var model = new ProductViewModel()
            {
                Product = product,
            };
            return View(model);
        }

        #endregion

        #region AddCommentToProduct

        [HttpPost]
        public async Task<IActionResult> AddCommentToProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var productId = Convert.ToInt32(TempData["productId"]);
                var product = _productService.GetProduct(productId);
                var result = await _productService.AddCommentToProduct(product, model.CommentMessage);
                model.Product = product;
                if (result)
                {
                    model.CommentMessage = null;
                    ViewData["SuccessMessage"] = "دیدگاه شما با موفقیت ثبت و بعد از تایید ادمین نمایش داده خواهد شد";
                    ModelState.Clear();
                    return View("ProductDetails", model);
                }
                ModelState.AddModelError("", "مشکلی در زمان ارسال پیام رخ داد.");
            }
            return View("ProductDetails", model);
        }

        #endregion

    }
}
