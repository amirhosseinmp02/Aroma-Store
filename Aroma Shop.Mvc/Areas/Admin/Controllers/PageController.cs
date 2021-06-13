using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Domain.Models.PageModels;
using Aroma_Shop.Domain.Models.ProductModels;

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
        public IActionResult Index(int pageNumber = 1, string search = null)
        {
            IEnumerable<Page> pages;

            if (!string.IsNullOrEmpty(search))
            {
                pages =
                    _pageService.GetPages().Where(p =>
                        p.PageTitle.Contains(search) ||
                        p.PageLink.Contains(search));

                ViewBag.search = search;
            }
            else
                pages =
                    _pageService.GetPages();

            if (!pages.Any())
            {
                ViewBag.isEmpty = true;

                return View();
            }

            var page =
                new Paging<Page>(pages, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var pagesPage =
                page.QueryResult;

            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;

            return View(pagesPage);
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

        #region EditPage

        [HttpGet("/Admin/Pages/EditPage")]
        public IActionResult EditPage(int pageId)
        {
            var page =
                _pageService.GetPage(pageId);

            if (page == null)
                return NotFound();

            return View(page);
        }

        #endregion

        #region DeletePage

        [HttpGet("Admin/Pages/DeletePage")]
        public IActionResult DeletePage(int pageId)
        {
            var result =
                _pageService.DeletePageById(pageId);

            if (result)
                return RedirectToAction("Index");

            return NotFound();
        }

        #endregion
    }
}
