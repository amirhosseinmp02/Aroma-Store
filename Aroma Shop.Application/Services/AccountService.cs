using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Aroma_Shop.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Aroma_Shop.Application.Utilites;
using Aroma_Shop.Application.ViewModels.Account;
using Aroma_Shop.Application.ViewModels.User;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Aroma_Shop.Domain.Models.UserModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Aroma_Shop.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly RoleManager<CustomIdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _accessor;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<CustomIdentityUser> userManager, SignInManager<CustomIdentityUser> signInManager, LinkGenerator linkGenerator, IHttpContextAccessor accessor, IEmailService emailService, RoleManager<CustomIdentityRole> roleManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _linkGenerator = linkGenerator;
            _accessor = accessor;
            _emailService = emailService;
            _roleManager = roleManager;
            _userRepository = userRepository;
        }

        public async Task<IdentityResult> CreateUser(CustomIdentityUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            await _userManager.AddToRoleAsync(user, "Customer");

            return result;
        }

        public async Task<bool> SendEmailConfirmation(CustomIdentityUser user, string returnController, string returnAction)
        {
            try
            {
                var emailConfirmationToken =
                    await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var url = _linkGenerator.GetUriByAction(_accessor.HttpContext, returnAction, returnController,
                    new { email = user.Email, token = emailConfirmationToken }
                    , _accessor.HttpContext.Request.Scheme);

                var emailMessage =
                    await ViewToStringRenderer
                        .RenderViewToStringAsync(_accessor.HttpContext.RequestServices
                            , $"~/Views/Emails/EmailConfirmationTemplate.cshtml", url);

                await
                    _emailService.SendEmailAsync
                        (user.Email, "تأیید ایمیل", emailMessage.ToString(), true);
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        public async Task<bool> EmailConfirmation(string email, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                    return false;

                var user =
                    await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return false;
                var result =
                    await _userManager.ConfirmEmailAsync(user, token);

                return result.Succeeded;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        public async Task<JsonResult> IsUserNameExist(string userName)
        {
            var user =
                await _userManager.FindByNameAsync(userName);

            if (user == null)
                return new JsonResult(true);

            return new JsonResult("امکان استفاده از این نام کاربری وجود ندارد");
        }

        public async Task<JsonResult> IsEmailExist(string email)
        {
            var emaill =
                await _userManager.FindByEmailAsync(email);

            if (emaill == null)
                return new JsonResult(true);

            return new JsonResult("امکان استفاده از این ایمیل وجود ندارد");
        }

        public bool IsUserSignedIn(ClaimsPrincipal user)
        {
            if (_signInManager.IsSignedIn(user))
                return true;

            return false;
        }

        public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthentications()
        {
            var externalLogins =
                await _signInManager.GetExternalAuthenticationSchemesAsync();

            return externalLogins;
        }

        public ChallengeResult ConfigureExternalLogins(string provider, string controllerName, string actionName, string returnUrl)
        {
            var redirectUrl =
                _linkGenerator
                    .GetUriByAction
                    (_accessor.HttpContext
                        , actionName, controllerName
                        , new { returnurl = returnUrl }
                        , _accessor.HttpContext.Request.Scheme);

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        public async Task<bool> ConfigureExternalLoginsCallBacks(string remoteError = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(remoteError))
                    return false;

                var externalLoginInfo =
                    await _signInManager.GetExternalLoginInfoAsync();

                if (externalLoginInfo == null)
                    return false;

                var signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
                    externalLoginInfo.ProviderKey, false, true);

                if (signInResult.Succeeded)
                    return true;

                var email =
                    externalLoginInfo.Principal
                        .FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user =
                        await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        var userName = email.Split("@")[0];

                        user = new CustomIdentityUser()
                        {
                            UserName = (userName.Length <= 10 ? userName : userName.Substring(0, 7)),
                            Email = email,
                            EmailConfirmed = true
                        };

                        await _userManager.CreateAsync(user);
                    }
                    await _userManager.AddLoginAsync(user, externalLoginInfo);

                    await _signInManager.SignInAsync(user, false);

                    return true;
                }

                return false;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        public async Task<bool> LoginWithPassword(LoginViewModel vm)
        {
            try
            {
                var user =
                    await _userManager.FindByEmailAsync(vm.Email);

                if (user == null)
                    return false;

                var result =
                    await _signInManager.PasswordSignInAsync
                    (user.UserName, vm.Password
                        , vm.RememberMe, false);

                return result.Succeeded;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        public async Task<bool> LogOutUser()
        {
            try
            {
                await _signInManager.SignOutAsync();

                return true;

            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        public async Task<bool> SendRestPasswordLink(string userEmail, string returnController, string returnAction)
        {
            try
            {
                var user =
                    await _userManager.FindByEmailAsync(userEmail);
                if (user == null)
                    return false;

                var restPasswordToken =
                    await _userManager.GeneratePasswordResetTokenAsync(user);

                var resetPasswordUrl = _linkGenerator.GetUriByAction(_accessor.HttpContext, returnAction, returnController,
                    new { email = user.Email, token = restPasswordToken }
                    , _accessor.HttpContext.Request.Scheme);

                var emailMessage =
                    await ViewToStringRenderer
                        .RenderViewToStringAsync(_accessor.HttpContext.RequestServices
                            , $"~/Views/Emails/RestPasswordEmailTemplate.cshtml", resetPasswordUrl);

                await
                    _emailService.SendEmailAsync
                        (user.Email, "تغییر کلمه عبور", emailMessage.ToString(), true);
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        public async Task<bool> RestPassword(string userEmail, string token, string newPassword)
        {
            try
            {
                var user =
                    await _userManager.FindByEmailAsync(userEmail);
                if (user == null)
                    return false;

                var result =
                    await _userManager.ResetPasswordAsync(user, token, newPassword);

                return result.Succeeded;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        public async Task<IEnumerable<UserViewModel>> GetUsers(ClaimsPrincipal currentUser)
        {
            var loggedUserId =
                currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedUser =
                await _userManager.FindByIdAsync(loggedUserId);
            var loggedUserRole =
                GetUserRole(loggedUser);

            var users =
                _userManager.Users.ToList();

            IEnumerable<UserViewModel> result;

            if (loggedUserRole == "Founder")
            {
                result = users.Where(p =>
                        !_userManager.IsInRoleAsync(p, "Founder").Result)
                    .Select(p => new UserViewModel()
                    {
                        UserId = p.Id,
                        UserName = p.UserName,
                        UserEmail = p.Email,
                        UserRoleName =
                        _userManager.GetRolesAsync(p).Result.FirstOrDefault()
                    });
            }
            else
            {
                result = users.Where(p =>
                    !_userManager.IsInRoleAsync(p, "Founder").Result &&
                    !_userManager.IsInRoleAsync(p, "Manager").Result).Select(p => new UserViewModel()
                    {
                        UserId = p.Id,
                        UserName = p.UserName,
                        UserEmail = p.Email,
                        UserRoleName =
                         _userManager.GetRolesAsync(p).Result.FirstOrDefault()
                    });
            }

            return result;
        }

        public async Task<CreateEditUserViewModel> GetUser(ClaimsPrincipal currentUser, string userId)
        {
            var requestedUser = 
                _userManager.Users
                    .Where(p => p.Id == userId)
                    .Include(p => p.UserDetail)
                    .FirstOrDefault();

            var roles = 
                await GetRolesForEdit(currentUser);

            var user = new CreateEditUserViewModel()
            {
                UserName = requestedUser.UserName,
                Email = requestedUser.Email,
                Roles = roles,
                UserRole = _userManager.GetRolesAsync(requestedUser).Result.FirstOrDefault(),
                FirstName = requestedUser.UserDetail.FirstName,
                LastName = requestedUser.UserDetail.LastName,
                UserProvince = requestedUser.UserDetail.UserProvince,
                UserCity = requestedUser.UserDetail.UserCity,
                UserAddress = requestedUser.UserDetail.UserAddress,
                UserZipCode = requestedUser.UserDetail.UserZipCode
            };

            return user;
        }

        public async Task<bool> DeleteUser(ClaimsPrincipal currentUser, string userId)
        {
            try
            {
                var loggedUserId =
                    currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
                var loggedUser =
                    await _userManager.FindByIdAsync(loggedUserId);

                var loggedUserRole =
                    GetUserRole(loggedUser);

                var requestedUser =
                    _userManager.Users
                        .Where(p => p.Id == userId)
                        .Include(p => p.UserDetail)
                        .FirstOrDefault();

                if (requestedUser == null)
                    return false;

                var requestedUserRole =
                    GetUserRole(requestedUser);

                if ((loggedUserRole == "Founder" && requestedUserRole == "Founder")
                    || ((loggedUserRole == "Manager") &&
                        (requestedUserRole == "Founder" || requestedUserRole == "Manager")))
                    return false;

                _userRepository.DeleteUserDetail(requestedUser.UserDetail);
                var result = 
                    await _userManager.DeleteAsync(requestedUser);
                _userRepository.Save();

                return result.Succeeded;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetRolesForEdit(ClaimsPrincipal currentUser)
        {
            var loggedUserId =
                currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedUser =
                await _userManager.FindByIdAsync(loggedUserId);

            var loggedUserRole =
                GetUserRole(loggedUser);

            var roles =
                _roleManager.Roles;

            IEnumerable<SelectListItem> result;

            if (loggedUserRole == "Founder")
            {
                result = roles.Where(p => p.Name != "Founder")
                    .Select(p =>
                        new SelectListItem(p.PersianName, p.Name));
            }
            else
            {
                result = roles.Where(p =>
                        p.Name != "Founder" && p.Name != "Manager")
                    .Select(p =>
                        new SelectListItem(p.PersianName, p.Name));
            }

            return result.OrderBy(p => p.Value);
        }

        public async Task<IdentityResult> CreateUserByAdmin(ClaimsPrincipal currentUser, CreateEditUserViewModel userViewModel)
        {
            var user = new CustomIdentityUser()
            {
                UserName = userViewModel.UserName,
                Email = userViewModel.Email,
                EmailConfirmed = true
            };

            var result =
                await _userManager.CreateAsync(user, userViewModel.UserPassword);

            var loggedUserId =
                currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedUser =
                await _userManager.FindByIdAsync(loggedUserId);

            var loggedUserRole =
                GetUserRole(loggedUser);

            string userRole;

            if ((loggedUserRole == "Founder" && userViewModel.UserRole == "Founder")
                || ((loggedUserRole == "Manager") && (userViewModel.UserRole == "Founder" || userViewModel.UserRole == "Manager"))
                || (userViewModel.UserRole != "Manager" && userViewModel.UserRole != "Writer" && userViewModel.UserRole != "Customer"))
            {
                userRole = "Customer";
            }
            else
            {
                userRole = userViewModel.UserRole;
            }

            await _userManager.AddToRoleAsync(user, userRole);

            var userDetail = new UserDetail()
            {
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                UserProvince = userViewModel.UserProvince,
                UserCity = userViewModel.UserCity,
                UserAddress = userViewModel.UserAddress,
                UserZipCode = userViewModel.UserZipCode
            };
            _userRepository.AddUserDetail(userDetail);

            user.UserDetail = userDetail;

            _userRepository.Save();

            return result;
        }

        public Task<IdentityResult> EditUserByAdmin(ClaimsPrincipal currentUser, CreateEditUserViewModel userViewModel)
        {
            throw new NotImplementedException();
        }

        //Utilities Methods

        private string GetUserRole(CustomIdentityUser user)
        {
            var userRole = 
                _userManager.GetRolesAsync(user)
                    .Result.FirstOrDefault();

            return userRole;
        }
    }
}
