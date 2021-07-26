using Aroma_Shop.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.Home;
using Aroma_Shop.Domain.Models.MediaModels;

namespace Aroma_Shop.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IMediaService _mediaService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IMediaService mediaService)
        {
            _logger = logger;
            _productService = productService;
            _mediaService = mediaService;
        }

        public IActionResult Index()
        {
            var banners =
                _mediaService
                    .GetBanners();

            var products = 
                _productService.GetProducts();

            var indexViewModel = new IndexViewModel()
            {
                Banners = banners,
                Products = products
            };

            return View(indexViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
