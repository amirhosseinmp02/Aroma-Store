using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PageController : Controller
    {
        private readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        #region ShowPages

        [HttpGet("/Admin/Pages")]
        public IActionResult Index()
        {
            var pages =
                _pageService.GetPages();

            return View(pages);
        }

        #endregion

        #region AddPage

        [HttpGet("/Admin/Pages/AddPage")]
        public IActionResult AddPage()
        {
            return View();
        }

        #endregion
    }
}
