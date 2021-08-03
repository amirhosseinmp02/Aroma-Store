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
        public IActionResult Visitors(int pageNumber = 1, string search = null)
        {
            IEnumerable<Visitor> visitors;

            if (!string.IsNullOrEmpty(search))
            {
                visitors =
                    _visitorService
                        .GetVisitors().Where(p =>
                            p.VisitorIpAddress.Contains(search));

                ViewBag.search = search;
            }
            else
                visitors =
                    _visitorService
                        .GetVisitors();

            if (!visitors.Any())
            {
                ViewBag.isEmpty = true;

                return View();
            }

            var page =
                new Paging<Visitor>(visitors, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var visitorsPage =
                page.QueryResult;

            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;

            return View(visitorsPage);
        }

        #endregion
    }
}
