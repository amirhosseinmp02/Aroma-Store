using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.Page;
using Aroma_Shop.Domain.Models.PageModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Authorization;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Authorize(Policy = "Writer")]
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
        public async Task<IActionResult> Index(int pageNumber = 1, string search = null)
        {
            var pages =
                await _pageService
                    .GetPagesAsync();

            if (!string.IsNullOrEmpty(search))
            {
                pages =
                    pages
                        .Where(p =>
                        p.PageTitle.Contains(search) ||
                        p.PagePathAddress.Contains(search));

                ViewBag.search = search;
            }

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

        #region CreatePage

        [HttpGet("/Admin/Pages/AddPage")]
        public IActionResult CreatePage()
        {
            return View();
        }

        [HttpPost("/Admin/Pages/AddPage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePage(AddPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _pageService
                        .CreatePageAsync(model);

                if (result == PageCreateUpdateResult.Successful)
                {
                    ModelState.Clear();

                    ViewData["SuccessMessage"] = "صفحه مورد نظر با موفقیت ساخته شد";

                    return View();
                }

                if(result == PageCreateUpdateResult.PathAddressExist)
                    ModelState.AddModelError("", "این آدرس صفحه در حال حاضر موجود است");

                else if(result == PageCreateUpdateResult.Failed)
                    ModelState.AddModelError("", "مشکلی در زمان ساخت صفحه رخ داد");
            }

            return View(model);
        }

        #endregion

        #region EditPage

        [HttpGet("/Admin/Pages/EditPage")]
        public async Task<IActionResult> EditPage(int pageId)
        {
            var page =
                await _pageService
                    .GetPageAsync(pageId);

            if (page == null)
                return NotFound();

            var model = new EditPageViewModel()
            {
                PageId = page.PageId,
                PageTitle = page.PageTitle,
                PagePathAddress = page.PagePathAddress,
                PageDescription = page.PageDescription
            };

            TempData["pageId"] = pageId;

            return View(model);
        }

        [HttpPost("/Admin/Pages/EditPage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPage(EditPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.PageId =
                    Convert.ToInt32(TempData["pageId"]);

                var result =
                    await _pageService
                        .UpdatePageAsync(model);

                if (result == PageCreateUpdateResult.Successful)
                    return RedirectToAction("Index");

                if (result == PageCreateUpdateResult.PathAddressExist)
                    ModelState.AddModelError("", "این آدرس صفحه در حال حاضر موجود است");

                else if (result == PageCreateUpdateResult.Failed)
                    ModelState.AddModelError("", "مشکلی در زمان ویرایش صفحه رخ داد");
            }

            TempData.Keep("pageId");

            return View(model);
        }

        #endregion

        #region DeletePage

        [HttpGet("Admin/Pages/DeletePage")]
        public async Task<IActionResult> DeletePage(int pageId)
        {
            var result =
                await _pageService.DeletePageByIdAsync(pageId);

            if (result)
                return RedirectToAction("Index");

            return NotFound();
        }

        #endregion

        #region IsPagePathAddressExist

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsPagePathAddressExistForAdd(string pagePathAddress)
        {
            var isPagePathAddressExist =
                await _pageService.IsPagePathAddressExistForAddJsonResultAsync(pagePathAddress);

            return isPagePathAddressExist;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsPagePathAddressExistForEdit(string pagePathAddress, int pageId)
        {
            var isPagePathAddressExist =
                await _pageService.IsPagePathAddressExistForEditJsonResultAsync(pagePathAddress, pageId);

            return isPagePathAddressExist;
        }

        #endregion
    }
}
