using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ProductController(IProductService productService, IMediaService mediaService)
        {
            _productService = productService;
            _mediaService = mediaService;
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
                                    .Contains(new Category() { CategoryName = Search }));

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
                            : p.MixedProductAttributes.Min(t => t.MixedProductAttributePrice) >= minimumPrice && p.MixedProductAttributes
                                .Min(t => t.MixedProductAttributePrice) <= maximumPrice)
                        .Where(p => p.IsSimpleProduct ? p.ProductQuantityInStock != 0 : p.MixedProductAttributes.Any(t => t.MixedProductAttributeQuantityInStock != 0));

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
                                    p.MixedProductAttributes.Max(t => t.MixedProductAttributeQuantityInStock)))
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
                                    p.MixedProductAttributes.Max(t => t.MixedProductAttributeQuantityInStock)))
                        .ThenBy(p =>
                            p.IsSimpleProduct
                                ? p.ProductPrice
                                : ((p.MixedProductAttributes.Any(t => t.MixedProductAttributeQuantityInStock != 0)
                                       ? p.MixedProductAttributes
                                           .Where(t => t.MixedProductAttributeQuantityInStock != 0)
                                           .Min(t => t.MixedProductAttributePrice)
                                       : p.MixedProductAttributes.Min(t => t.MixedProductAttributePrice)) +
                                   (p.MixedProductAttributes.Any(t => t.MixedProductAttributeQuantityInStock != 0)
                                       ? p.MixedProductAttributes
                                           .Where(t => t.MixedProductAttributeQuantityInStock != 0)
                                           .Max(t => t.MixedProductAttributePrice)
                                       : p.MixedProductAttributes.Max(t => t.MixedProductAttributePrice)) / 2));
            }
            else if (SortBy == "Price-Most-Expensive")
            {
                products =
                    products
                        .OrderByDescending(p =>
                            p.IsSimpleProduct
                                ? Convert.ToBoolean(p.ProductQuantityInStock)
                                : Convert.ToBoolean(
                                    p.MixedProductAttributes.Max(t => t.MixedProductAttributeQuantityInStock)))
                        .ThenByDescending(p =>
                            p.IsSimpleProduct
                                ? p.ProductPrice
                                : ((p.MixedProductAttributes.Any(t => t.MixedProductAttributeQuantityInStock != 0)
                                       ? p.MixedProductAttributes
                                           .Where(t => t.MixedProductAttributeQuantityInStock != 0)
                                           .Min(t => t.MixedProductAttributePrice)
                                       : p.MixedProductAttributes.Min(t => t.MixedProductAttributePrice)) +
                                   (p.MixedProductAttributes.Any(t => t.MixedProductAttributeQuantityInStock != 0)
                                       ? p.MixedProductAttributes
                                           .Where(t => t.MixedProductAttributeQuantityInStock != 0)
                                           .Max(t => t.MixedProductAttributePrice)
                                       : p.MixedProductAttributes.Max(t => t.MixedProductAttributePrice)) / 2));
            }
            else if (SortBy == "Popularity")
            {
                products =
                    products
                        .OrderByDescending(p =>
                            p.IsSimpleProduct
                                ? Convert.ToBoolean(p.ProductQuantityInStock)
                                : Convert.ToBoolean(
                                    p.MixedProductAttributes.Max(t => t.MixedProductAttributeQuantityInStock)))
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
