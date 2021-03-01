﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.Account;
using Aroma_Shop.Domain.Models.CustomIdentityModels;

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

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (_accountService.IsUserSignedIn(User))
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                var user = new CustomIdentityUser()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                var result = 
                    await _accountService.CreateUser(user, model.Password);
                if (result.Succeeded)
                {
                    var emailConfirmation = 
                        _accountService
                            .SendEmailConfirmation(user,"Account", "EmailConfirmation");
                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            return View(model);
        }

        #endregion

        #region Login

        [HttpGet("Login")]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if(_accountService.IsUserSignedIn(User))
                return RedirectToAction("Index", "Home");
            var model = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                ExternalsLogins = (await _accountService.GetExternalAuthentications()).ToList()
            };
            return View(model);
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login
            (LoginViewModel model, string returnUrl = null)
        {
            if (_accountService.IsUserSignedIn(User))
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }
        #endregion

        #region ExternalLogins

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogins(string provider, string returnUrl)
        {
            var result = 
                _accountService
                    .ConfigureExternalLogins
                        (provider, "Account"
                        , "ExternalLoginsCallBacks", returnUrl);
            return result;
        }

        public async Task<IActionResult> ExternalLoginsCallBacks
            (string returnUrl = null, string remoteError = null)
        {
            returnUrl =
                (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Content("~/"));
            var result = 
                await _accountService
                    .ConfigureExternalLoginsCallBacks(remoteError);
            if (result)
                return Redirect(returnUrl);
            return RedirectToAction("Login");
        }

        #endregion

        #region CheckingUser&Email

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsUserNameExist(string userName)
        {
            return await _accountService.IsUserNameExist(userName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsEmailExist(string email)
        {
            return await _accountService.IsEmailExist(email);
        }

        #endregion

        #region EmailConfirmation

        public async Task<IActionResult> EmailConfirmation(string email, string token)
        {
            var result = 
                await _accountService.EmailConfirmation(email, token);
            if (result)
                return View();
            return NotFound();
        }

        #endregion
    }
}
