using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.AdminArea;
using Microsoft.AspNetCore.Authorization;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Authorize(Policy = "Writer")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMediaService _mediaService;
        private readonly IProductService _productService;
        private readonly IVisitorService _visitorService;

        public HomeController(IAccountService accountService, IMediaService mediaService, IProductService productService, IVisitorService visitorService)
        {
            _accountService = accountService;
            _mediaService = mediaService;
            _productService = productService;
            _visitorService = visitorService;
        }

        public IActionResult Index()
        {
            var usersCount =
                _accountService
                    .GetUsersCount();

            var productsCount =
                _productService
                    .GetProductsCount();

            var completedOrdersCount =
                _productService
                    .GetCompletedOrdersCount();

            var messagesCount =
                _mediaService.GetMessagesCount();

            var numberOfVisits =
                _visitorService
                    .GetNumberOfVisits();

            var unCompletedOrdersCount =
            _productService
                .GetUnCompletedOrdersCount();

            var comments =
                _mediaService
                    .GetComments();

            var adminDashbordViewModel = new AdminDashbordViewModel()
            {
                UsersCount = usersCount,
                ProductsCount = productsCount,
                CompletedOrdersCount = completedOrdersCount,
                MessagesCount = messagesCount,
                NumberOfVisits = numberOfVisits,
                UnCompletedOrdersCount = unCompletedOrdersCount,
                Comments = comments
            };

            return View(adminDashbordViewModel);
        }
    }
}
