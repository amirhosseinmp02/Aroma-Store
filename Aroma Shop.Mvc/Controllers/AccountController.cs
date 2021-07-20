using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels.Account;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Microsoft.AspNetCore.Authorization;

namespace Aroma_Shop.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IProductService _productService;

        public AccountController(IAccountService accountService, IProductService productService)
        {
            _accountService = accountService;
            _productService = productService;
        }

        #region Register

        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (_accountService.IsUserSignedIn())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (_accountService.IsUserSignedIn())
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
                            .SendEmailConfirmation(user, "Account", "EmailConfirmation");

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
            if (_accountService.IsUserSignedIn())
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
            if (_accountService.IsUserSignedIn())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var result =
                    await _accountService.LoginWithPassword(model);

                if (result)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
            }
            model.ReturnUrl = returnUrl;

            model.ExternalsLogins =
                (await _accountService.GetExternalAuthentications()).ToList();

            ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است.");

            return View(model);
        }
        #endregion

        #region ManageAccount

        [Authorize]
        [HttpGet("/My-Account")]
        public IActionResult ManageAccount()
        {
            return View();
        }

        #endregion

        #region EditAccount

        [Authorize]
        [HttpGet("/My-Account/Edit-Account")]
        public async Task<IActionResult> EditAccount()
        {
            var loggedUser =
                await _accountService.GetLoggedUserWithDetails();

            var editAccountViewModel = new EditAccountViewModel()
            {
                FirstName = loggedUser.UserDetails.FirstName,
                LastName = loggedUser.UserDetails.LastName,
                UserName = loggedUser.UserName,
                Email = loggedUser.Email,
                UserProvince = loggedUser.UserDetails.UserProvince,
                UserCity = loggedUser.UserDetails.UserCity,
                UserAddress = loggedUser.UserDetails.UserAddress,
                UserZipCode = loggedUser.UserDetails.UserZipCode
            };

            return View(editAccountViewModel);
        }

        [Authorize]
        [HttpPost("/My-Account/Edit-Account")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(EditAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _accountService.EditAccount(model);

                if (result.Succeeded)
                {
                    ModelState.Clear();
                    ViewData["SuccessMessage"] = "اطلاعات با موفقیت ویرایش شد ، در صورت تغییر یا افزودن کلمه عبور باید دوباره وارد شوید";
                }

                else
                {
                    foreach (var identityError in result.Errors)
                    {
                        ModelState.AddModelError("", identityError.Description);
                    }
                }
            }

            return View(model);
        }

        #endregion

        #region ShowFavoriteProducts

        [Authorize]
        [HttpGet("/My-Account/Favorite-Products")]
        public async Task<IActionResult> FavoriteProducts()
        {
            var loggedUserFavoriteProducts =
                await _productService.GetLoggedUserFavoriteProducts();

            return View(loggedUserFavoriteProducts);
        }

        #endregion

        #region LogOut

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            var result =
                await _accountService.LogOutUser();

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region ForgotPassword

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _accountService
                        .SendRestPasswordLink(model.Email, "Account", "ResetPassword");

                ViewData["SuccessMessage"] =
                    "در صورت معتبر بودن ایمیل وارد شده ، لینک تغییر کلمه عبور به ایمیل شما ارسال خواهد شد.";

                ModelState.Clear();

                return View();
            }

            return View(model);
        }

        #endregion

        #region ResetPassword

        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Index", "Home");

            var model = new RestPasswordViewModel()
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestPassword(RestPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _accountService.RestPassword(model.Email, model.Token, model.NewPassword);

                if (result)
                {
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("", "مشکلی در زمان تغییر کلمه عبور رخ داد.");
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
            var isUserNameExist =
                await _accountService.IsUserNameExist(userName);

            return isUserNameExist;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsEmailExist(string email)
        {
            var isEmailExist =
                await _accountService.IsEmailExist(email);

            return isEmailExist;
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
