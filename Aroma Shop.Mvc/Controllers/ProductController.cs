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
            var product = 
                _productService.GetProduct(productId);

            product.Comments = product.Comments
                .Where(p => p.IsConfirmed && p.ParentComment == null).ToList();

            var model = new ProductViewModel()
            {
                Product = product,
            };

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
                    result = await _productService.AddCommentToProduct(model);
                else
                    result = await _productService.AddReplyToProductComment(model);

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

                    return View("ProductDetails", model);
                }

                ModelState.AddModelError("", "مشکلی در زمان ثبت دیدگاه / پاسخ رخ داد.");
            }

            return View("ProductDetails", model);
        }

        #endregion

    }
}
