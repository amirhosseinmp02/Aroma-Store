using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Domain.Models.VisitorModels;
using Microsoft.AspNetCore.Authorization;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Authorize(Policy = "Writer")]
    [Area("Admin")]
    public class ReportController : Controller
    {
        private readonly IVisitorService _visitorService;

        public ReportController(IVisitorService visitorService)
        {
            _visitorService = visitorService;
        }

        #region ShowVisitors

        [HttpGet("/Admin/Visitors")]
        public async Task<IActionResult> Visitors(int pageNumber = 1, string search = null)
        {
            var visitors =
                await _visitorService
                    .GetVisitorsAsync();

            if (!string.IsNullOrEmpty(search))
            {
                visitors = 
                    visitors
                        .Where(p =>
                            p.VisitorIpAddress.Contains(search));
            }

            if (!visitors.Any())
            {
                ViewData["isEmpty"] = true;

                return View();
            }

            var page =
                new Paging<Visitor>(visitors, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var visitorsPage =
                page.QueryResult;

            ViewData["pageNumber"] = pageNumber;
            ViewData["firstPage"] = page.FirstPage;
            ViewData["lastPage"] = page.LastPage;
            ViewData["prevPage"] = page.PreviousPage;
            ViewData["nextPage"] = page.NextPage;
            ViewData["search"] = search;
            ViewData["isEmpty"] = false;

            return View(visitorsPage);
        }

        #endregion
    }
}
