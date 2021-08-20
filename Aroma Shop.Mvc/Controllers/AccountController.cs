using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.ViewModels;
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
                    await _accountService.CreateUserAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var emailConfirmation =
                        await _accountService
                            .SendEmailConfirmationAsync(user, "Account", "EmailConfirmation");

                    ViewData["SuccessMessage"] = "ثبت نام با موفقیت انجام شد ، لطفا ایمیل خود را بررسی بفرمایید";

                    return View();
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
                ExternalsLogins = (await _accountService.GetExternalAuthenticationsAsync()).ToList()
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
                    await _accountService
                        .LoginWithPasswordAsync(model);

                if (result == LoginWithPasswordResult.Successful)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("",
                    result == LoginWithPasswordResult.EmailNotConfirmed
                        ? "برای ورود به حساب ، باید ایمیل خود را تایید کنید"
                        : "نام کاربری یا رمز عبور اشتباه است.");
            }
            model.ReturnUrl = returnUrl;

            model.ExternalsLogins =
                (await _accountService.GetExternalAuthenticationsAsync()).ToList();

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
                await _accountService
                    .GetLoggedUserWithDetailsAsync();

            var editAccountViewModel = new EditAccountViewModel()
            {
                FirstName = loggedUser.UserDetails.FirstName,
                LastName = loggedUser.UserDetails.LastName,
                UserName = loggedUser.UserName,
                Email = loggedUser.Email,
                MobileNumber = loggedUser.MobileNumber,
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
                    await _accountService.EditAccountAsync(model);

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
                await _productService
                    .GetLoggedUserFavoriteProductsAsync();

            return View(loggedUserFavoriteProducts);
        }

        #endregion

        #region ShowAccountOrders

        [Authorize]
        [HttpGet("/My-Account/Orders")]
        public async Task<IActionResult> AccountOrders()
        {
            var loggedUserOrders =
                await _productService
                    .GetLoggedUserOrdersAsync();

            var availableLoggedUserOrders =
                loggedUserOrders
                    .Where(p => p.NotEmpty);


            return View(loggedUserOrders);
        }

        #endregion

        #region ShowOrder

        [Authorize]
        [HttpGet("/My-Account/Order/{orderId}")]
        public async Task<IActionResult> AccountOrder(int orderId)
        {
            var orderViewModel =
                await _productService
                    .GetLoggedUserOrderInvoiceAsync(orderId);

            if (orderViewModel == null)
                return NotFound();

            ViewData["HeaderTitle"] = "مشاهده سفارش";

            ViewData["Message"] = $"فاکتور شماره {orderViewModel.OrderId} فروشگاه آروما";

            return View("/Views/Product/OrderConfirmation.cshtml", orderViewModel);
        }

        #endregion

        #region LogOut

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            var result =
                await _accountService.LogOutUserAsync();

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
                        .SendRestPasswordLinkAsync(model.Email, "Account", "ResetPassword");

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
                    await _accountService
                        .RestPasswordAsync(model.Email, model.Token, model.NewPassword);

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
                    .ConfigureExternalLoginsCallBacksGetUriByActionAsync(remoteError);

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
                await _accountService.IsUserNameExistAsync(userName);

            return isUserNameExist;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsEmailExist(string email)
        {
            var isEmailExist =
                await _accountService.IsEmailExistAsync(email);

            return isEmailExist;
        }

        #endregion

        #region EmailConfirmation

        public async Task<IActionResult> EmailConfirmation(string email, string token)
        {
            var result =
                await _accountService.EmailConfirmationAsync(email, token);

            if (result)
                return View();

            return NotFound();
        }

        #endregion
    }
}
