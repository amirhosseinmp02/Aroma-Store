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

        [HttpGet("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            if(_accountService.IsUserSignedIn(User))
                return RedirectToAction("Index", "Home");
            var model = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                ExternalsLogins = 
            }
            return View();
        }

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
