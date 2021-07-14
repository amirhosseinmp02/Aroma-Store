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
        public IActionResult Index(int pageNumber = 1, string sortby = null, string search = null, IEnumerable<int> categories = null)
        {
            IEnumerable<Product> products;

            if (!string.IsNullOrEmpty(search))
            {
                products = _productService.GetProducts()
                    .Where(p => p.ProductName.Contains(search)
                                || p.Categories
                                    .Contains(new Category() { CategoryName = search }));

                ViewBag.search = search;
            }
            else
                products =
                    _productService
                        .GetProducts();

            if (!products.Any())
            {
                ViewBag.isEmpty = true;

                return View();
            }

            var page =
                new Paging<Product>(products, 12, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var productsPage =
                page.QueryResult;

            if (sortby == "newest" || string.IsNullOrEmpty(sortby))
            {
                productsPage =
                    productsPage
                        .OrderByDescending(p => p.RegistrationTime);
            }
            else if (sortby == "priceCheapest")
            {

            }
            else if (sortby == "priceMostExpensive")
            {

            }
            else if (sortby == "popularity")
            {
                productsPage =
                    productsPage
                        .OrderByDescending(p => p.ProductHits);
            }

            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;

            var allCategories =
                _productService.GetCategories();

            var productsViewModel = new ProductsViewModel()
            {
                Products = productsPage,
                Categories = allCategories
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
