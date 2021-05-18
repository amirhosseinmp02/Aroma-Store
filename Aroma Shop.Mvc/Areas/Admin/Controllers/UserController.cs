using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.User;
using Microsoft.AspNetCore.Authorization;

namespace Aroma_Shop.Mvc.Areas.Admin.Controllers
{
    [Authorize(Policy = "Manager")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IAccountService _accountService;

        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #region ShowUsers

        [HttpGet("/Admin/Users")]
        public async Task<IActionResult> Index(int pageNumber = 1, string search = null)
        {
            IEnumerable<UserViewModel> users;
            if (!string.IsNullOrEmpty(search))
            {
                users = _accountService.GetUsers(User)
                    .Result.Where(p => p.UserName.Contains(search) ||
                                       p.UserEmail.Contains(search) ||
                                       p.UserRoleName.Contains(search));
                ViewBag.search = search;
            }
            else
                users = await _accountService.GetUsers(User);
            if (!users.Any())
            {
                ViewBag.isEmpty = true;
                return View();
            }
            var page = new Paging<UserViewModel>(users, 11, pageNumber);
            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();
            var usersPage = page.QueryResult;
            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;
            return View(usersPage);
        }

        #endregion
    }
}
