using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;

namespace Aroma_Shop.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #region Register

        [HttpGet("Register")]
        public IActionResult Register()
        {
            if(_accountService.IsUserSignedIn(User))
                return RedirectToAction("Index", "Home");
            return View();
        }

        #endregion

        #region CheckingUser&Email
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IsUserNameExist(string userName)
        {
            return _accountService.IsUserNameExist(userName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IsEmailExist(string email)
        {
            return _accountService.IsEmailExist(email);
        }
        #endregion
    }
}
