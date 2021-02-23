using Aroma_Shop.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Domain.Models;

namespace Aroma_Shop.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageService _messageService;

        public HomeController(ILogger<HomeController> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/Contact-Us")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost("/Contact-Us")]
        public IActionResult ContactUs(Message model)
        {
            if (ModelState.IsValid)
            {
                var result = _messageService.AddMessage(model);
                if (result)
                {
                    ViewData["SuccessMessage"] = "پیام شما با موفقیت ارسال شد.";
                    ModelState.Clear();
                    return View();
                }
                ModelState.AddModelError("","مشکلی در زمان ارسال پیام رخ داد.");
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
