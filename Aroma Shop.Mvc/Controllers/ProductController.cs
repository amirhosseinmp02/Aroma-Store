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
        public IActionResult AddCommentToProduct(ProductViewModel model)
        {

        }

        #endregion

    }
}
