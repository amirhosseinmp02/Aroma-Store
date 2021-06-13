using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Domain.Models.PageModels;

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

        [HttpPost("/Admin/Pages/AddPage")]
        [ValidateAntiForgeryToken]
        public IActionResult AddPage(Page model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    _pageService.AddPage(model);

                if (result)
                {
                    ModelState.Clear();

                    ViewData["SuccessMessage"] = "صفحه مورد نظر با موفقیت ساخته شد";

                    return View();
                }

                ModelState.AddModelError("","مشکلی در زمان ساخت صفحه رخ داد");
            }

            return View(model);
        }

        #endregion
    }
}
