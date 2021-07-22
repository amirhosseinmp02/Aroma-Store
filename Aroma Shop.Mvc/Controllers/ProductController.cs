using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Authorization;

namespace Aroma_Shop.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMediaService _mediaService;
        private readonly IAccountService _accountService;

        public ProductController(IProductService productService, IMediaService mediaService, IAccountService accountService)
        {
            _productService = productService;
            _mediaService = mediaService;
            _accountService = accountService;
        }

        #region ShowProducts

        [HttpGet("/Products/")]
        public IActionResult Index(int pageNumber = 1, string SortBy = null, string Search = null, IEnumerable<int> SelectedCategories = null, int minimumPrice = 0, int maximumPrice = 0)
        {
            IEnumerable<Product> products;

            if (!string.IsNullOrEmpty(Search))
            {
                products = _productService.GetProducts()
                    .Where(p => p.ProductName.Contains(Search)
                                || p.Categories
                                    .Any(t => t.CategoryName.Contains(Search)));

                ViewBag.search = Search;
            }
            else
                products =
                    _productService
                        .GetProducts();

            if (SelectedCategories.Any())
            {
                products =
                    products
                        .Where(p => SelectedCategories.Any(t => p.Categories.Any(c => c.CategoryId == t)));
            }

            if (minimumPrice != 0 || maximumPrice != 0)
            {
                products =
                    products
                        .Where(p => p.IsSimpleProduct
                            ? p.ProductPrice >= minimumPrice &&
                              p.ProductPrice <= maximumPrice
                            : p.ProductVariations.Min(t => t.ProductVariationPrice) >= minimumPrice && p.ProductVariations
                                .Min(t => t.ProductVariationPrice) <= maximumPrice)
                        .Where(p => p.IsSimpleProduct ? p.ProductQuantityInStock != 0 : p.ProductVariations.Any(t => t.ProductVariationQuantityInStock != 0));

                ViewBag.minimumPrice = minimumPrice;
                ViewBag.maximumPrice = maximumPrice;
            }

            ProductsViewModel productsViewModel;

            var allCategories =
                _productService.GetCategories();

            if (!products.Any())
            {
                ViewBag.isEmpty = true;

                allCategories =
                    _productService.GetCategories();

                productsViewModel = new ProductsViewModel()
                {
                    Categories = allCategories,
                    SelectedCategories = SelectedCategories,
                    SortBy = SortBy
                };

                return View(productsViewModel);
            }

            if (SortBy == "Newest" || string.IsNullOrEmpty(SortBy))
            {
                products =
                    products
                        .OrderByDescending(p =>
                            p.IsSimpleProduct
                                ? Convert.ToBoolean(p.ProductQuantityInStock)
                                : Convert.ToBoolean(
                                    p.ProductVariations.Max(t => t.ProductVariationQuantityInStock)))
                        .ThenByDescending(p => p.RegistrationTime);
            }
            else if (SortBy == "Price-Cheapest")
            {
                products =
                    products
                        .OrderBy(p =>
                            p.IsSimpleProduct
                                ? Convert.ToBoolean(p.ProductQuantityInStock)
                                : Convert.ToBoolean(
                                    p.ProductVariations.Max(t => t.ProductVariationQuantityInStock)))
                        .ThenBy(p =>
                            p.IsSimpleProduct
                                ? p.ProductPrice
                                : ((p.ProductVariations.Any(t => t.ProductVariationQuantityInStock != 0)
                                       ? p.ProductVariations
                                           .Where(t => t.ProductVariationQuantityInStock != 0)
                                           .Min(t => t.ProductVariationPrice)
                                       : p.ProductVariations.Min(t => t.ProductVariationPrice)) +
                                   (p.ProductVariations.Any(t => t.ProductVariationQuantityInStock != 0)
                                       ? p.ProductVariations
                                           .Where(t => t.ProductVariationQuantityInStock != 0)
                                           .Max(t => t.ProductVariationPrice)
                                       : p.ProductVariations.Max(t => t.ProductVariationPrice)) / 2));
            }
            else if (SortBy == "Price-Most-Expensive")
            {
                products =
                    products
                        .OrderByDescending(p =>
                            p.IsSimpleProduct
                                ? Convert.ToBoolean(p.ProductQuantityInStock)
                                : Convert.ToBoolean(
                                    p.ProductVariations.Max(t => t.ProductVariationQuantityInStock)))
                        .ThenByDescending(p =>
                            p.IsSimpleProduct
                                ? p.ProductPrice
                                : ((p.ProductVariations.Any(t => t.ProductVariationQuantityInStock != 0)
                                       ? p.ProductVariations
                                           .Where(t => t.ProductVariationQuantityInStock != 0)
                                           .Min(t => t.ProductVariationPrice)
                                       : p.ProductVariations.Min(t => t.ProductVariationPrice)) +
                                   (p.ProductVariations.Any(t => t.ProductVariationQuantityInStock != 0)
                                       ? p.ProductVariations
                                           .Where(t => t.ProductVariationQuantityInStock != 0)
                                           .Max(t => t.ProductVariationPrice)
                                       : p.ProductVariations.Max(t => t.ProductVariationPrice)) / 2));
            }
            else if (SortBy == "Popularity")
            {
                products =
                    products
                        .OrderByDescending(p =>
                            p.IsSimpleProduct
                                ? Convert.ToBoolean(p.ProductQuantityInStock)
                                : Convert.ToBoolean(
                                    p.ProductVariations.Max(t => t.ProductVariationQuantityInStock)))
                        .ThenByDescending(p => p.ProductHits);
            }

            var page =
                new Paging<Product>(products, 12, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var productsPage =
                page.QueryResult;

            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;

            productsViewModel = new ProductsViewModel()
            {
                Products = productsPage,
                Categories = allCategories,
                SelectedCategories = SelectedCategories,
                SortBy = SortBy
            };

            return View(productsViewModel);
        }

        #endregion

        #region ProductDetails

        [HttpGet("/Products/{productId}")]
        public IActionResult ProductDetails(int productId)
        {
            var product =
                _productService.GetProduct(productId);

            if (product == null)
                return NotFound();

            _productService
                .AddHitsToProduct(product);

            product.Comments = product.Comments
                .Where(p => p.IsConfirmed && p.ParentComment == null).ToList();

            var model = new ProductViewModel()
            {
                Product = product,
            };

            return View(model);
        }

        #endregion

        #region ShoppingCart

        [Authorize]
        [HttpGet("/Shopping-Cart")]
        public async Task<IActionResult> ShoppingCart()
        {
            var loggedUserOpenOrder =
                await _accountService.GetLoggedUserOpenOrder();

            return View(loggedUserOpenOrder);
        }

        #endregion

        #region AddProductToCart

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductToCart(int productId, int requestedQuantity, int productVariationId)
        {
            var product =
                _productService.GetProduct(productId);

            if (product == null)
                return NotFound();

            var result =
                await _productService
                    .AddProductToCart(product, requestedQuantity, productVariationId);

            if (result == AddProductToCartResult.Successful)
            {
                return RedirectToAction("ShoppingCart");
            }

            if (result == AddProductToCartResult.OutOfStock)
            {
                ViewData["OutOfStockError"] = "موجودی کافی نیست";

                product.Comments = product.Comments
                    .Where(p => p.IsConfirmed && p.ParentComment == null).ToList();

                var model = new ProductViewModel()
                {
                    Product = product,
                };

                return View("ProductDetails", model);
            }

            return NotFound();
        }

        #endregion

        #region UpdateCart

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCart(IEnumerable<int> orderDetailsQuantities)
        {
            if (ModelState.IsValid)
            {
                var loggedUserOpenOrder =
                    await _accountService
                        .GetLoggedUserOpenOrder();

                var result =
                    await _productService
                        .UpdateCart(loggedUserOpenOrder, orderDetailsQuantities);

                if (result)
                {
                    ViewData["Message"] = "سبد خرید با موفقیت بروز شد";

                    return View("ShoppingCart", loggedUserOpenOrder);
                }
            }

            return NotFound();
        }

        #endregion

        #region RemoveProductFormCart

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveProductFormCart(int orderDetailsId)
        {
            if (ModelState.IsValid)
            {
                var result =
                    _productService
                        .DeleteOrderDetailsById(orderDetailsId);

                if (result)
                    return RedirectToAction("ShoppingCart");
            }

            return NotFound();
        }

        #endregion

        #region ApplyDiscountOnCart

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyDiscountOnCart(string discountCode)
        {
            var loggedUserOpenOrder =
                await _accountService
                    .GetLoggedUserOpenOrder();

            if (ModelState.IsValid)
            {
                var result =
                    await _productService
                        .AddDiscountToCart(loggedUserOpenOrder, discountCode);

                if (result)
                    ViewData["Message"] = "تخفیف با موفقیت اعمال شد";

                else
                    ViewData["Message"] = "کد تخفیف معتبر نیست";
            }

            return View("ShoppingCart", loggedUserOpenOrder);
        }

        #endregion

        #region ProceedToCheckout

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProceedToCheckout(IEnumerable<int> orderDetailsQuantities)
        {
            if (ModelState.IsValid)
            {
                var loggedUserOpenOrder =
                    await _accountService
                        .GetLoggedUserOpenOrder();

                var result =
                    await _productService
                        .UpdateCart(loggedUserOpenOrder, orderDetailsQuantities);

                if (result)
                    return RedirectToAction("CartCheckOut");
            }

            return NotFound();
        }

        #endregion

        #region CartCheckOut

        [Authorize]
        [HttpGet("/Cart-CheckOut")]
        public async Task<IActionResult> CartCheckOut()
        {
            var cartCheckOutViewModel =
                await _accountService
                    .GetLoggedUserCartCheckOut();

            if (cartCheckOutViewModel.Order == null)
                return NotFound();

            return View(cartCheckOutViewModel);
        }

        [Authorize]
        [HttpPost("/Cart-CheckOut")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CartCheckOut(CartCheckOutViewModel model)
        {
            if (ModelState.IsValid)
            {

            }

            var cartCheckOutViewModel =
                await _accountService
                    .GetLoggedUserCartCheckOut();

            return View(cartCheckOutViewModel);
        }

        #endregion

        #region OrderConfirmation

        [HttpGet("/Order-Confirmation")]
        [Authorize]
        public IActionResult OrderConfirmation()
        {
            return View();
        }

        #endregion

        #region AddProductToUserFavoriteProducts

        [Authorize]
        [HttpGet("/Products/AddToFavoriteProducts")]
        public async Task<IActionResult> AddProductToUserFavoriteProducts(int favoriteProductId)
        {
            var result =
                await _productService
                    .AddProductByIdToLoggedUserFavoriteProducts(favoriteProductId);

            if (result)
            {
                var returnUrl =
                    Request.Headers["Referer"].ToString();

                return Redirect(returnUrl);
            }

            return NotFound();

        }

        #endregion

        #region RemoveProductFromUserFavoriteProducts

        [Authorize]
        [HttpGet("/Products/RemoveFromFavoriteProducts")]
        public async Task<IActionResult> RemoveProductFromUserFavoriteProducts(int favoriteProductId)
        {
            var result =
                await _productService
                    .RemoveProductByIdFromLoggedUserFavoriteProducts(favoriteProductId);

            if (result)
            {
                var returnUrl =
                    Request.Headers["Referer"].ToString();

                return Redirect(returnUrl);
            }

            return NotFound();
        }

        #endregion
    }
}
