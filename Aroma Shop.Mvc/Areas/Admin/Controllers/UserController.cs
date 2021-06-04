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
                users = _accountService.GetUsers()
                    .Result.Where(p => p.UserName.Contains(search) ||
                                       p.UserEmail.Contains(search) ||
                                       p.UserRoleName.Contains(search));

                ViewBag.search = search;
            }
            else
                users = 
                    await _accountService.GetUsers();

            if (!users.Any())
            {
                ViewBag.isEmpty = true;

                return View();
            }

            var page = 
                new Paging<UserViewModel>(users, 11, pageNumber);

            if (pageNumber < page.FirstPage || pageNumber > page.LastPage)
                return NotFound();

            var usersPage = 
                page.QueryResult;

            ViewBag.pageNumber = pageNumber;
            ViewBag.firstPage = page.FirstPage;
            ViewBag.lastPage = page.LastPage;
            ViewBag.prevPage = page.PreviousPage;
            ViewBag.nextPage = page.NextPage;

            return View(usersPage);
        }

        #endregion

        #region UserDetails

        [HttpGet("/Admin/Users/{userId}")]
        public async Task<IActionResult> UserDetails(string userId)
        {
            var user =
                await _accountService.GetUser(userId);

            if (user == null)
                return NotFound();

            return View(user);
        }

        #endregion

        #region CreateUser

        [HttpGet("/Admin/Users/CreateUser")]
        public async Task<IActionResult> CreateUser()
        {
            var roles = 
                await _accountService.GetRoles();

            var model = new CreateUserViewModel()
            {
                Roles = roles
            };

            return View(model);
        }

        [HttpPost("/Admin/Users/CreateUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = 
                    await _accountService.CreateUserByAdmin( model);
                if (result.Succeeded)
                {
                    ModelState.Clear();

                    var roles = 
                        await _accountService.GetRoles();

                    model = new CreateUserViewModel()
                    {
                        Roles = roles
                    };

                    ViewData["SuccessMessage"] = "اکانت مورد نظر با موفقیت ساخته شد";

                    return View(model);
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            return View(model);
        }

        #endregion

        #region EditUser

        [HttpGet("/Admin/Users/EditUser")]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = 
                await _accountService.GetUserForEdit(userId);

            if (user == null)
                return NotFound();

            TempData["userId"] = user.UserId;

            return View(user);
        }

        [HttpPost("/Admin/Users/EditUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.UserId = 
                    TempData["userId"].ToString();

                var result = 
                    await _accountService.EditUserByAdmin(model);

                if (result.Succeeded)
                    return RedirectToAction("Index");

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            var roles = 
                await _accountService.GetRoles();

            model.Roles = roles;

            TempData.Keep("userId");

            return View(model);
        }

        #endregion

        #region DeleteUser

        [HttpGet("/Admin/Users/DeleteUser")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = 
                await _accountService.DeleteUser(userId);

            if (result)
                return RedirectToAction("Index");

            return NotFound();
        }

        #endregion
    }
}
