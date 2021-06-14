using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Microsoft.AspNetCore.Diagnostics;

namespace Aroma_Shop.Mvc.Controllers
{
    public class ManageErrors : Controller
    {
        private readonly IPageService _pageService;

        public ManageErrors(IPageService pageService)
        {
            _pageService = pageService;
        }

        public IActionResult Error404()
        {
            var feature = 
                HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            var originalPath =
                feature?.OriginalPath.Substring(1);

            var page =
                _pageService.GetPageByPathAddress(originalPath);

            if (page != null)
                return View("/Views/Home/Page.cshtml", page);

            return View();
        }
    }
}
