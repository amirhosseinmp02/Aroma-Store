using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Application.ViewModels;
using Aroma_Shop.Application.ViewModels.Account;
using Aroma_Shop.Application.ViewModels.Product;
using Aroma_Shop.Application.ViewModels.User;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IAccountService
    {
        bool IsUserSignedIn();
        Task<IdentityResult> CreateUserAsync(CustomIdentityUser user, string password);
        Task<bool> SendEmailConfirmationAsync(CustomIdentityUser user, string controllerName, string actionName);
        Task<bool> EmailConfirmationAsync(string email, string token);
        Task<JsonResult> IsUserNameExistAsync(string userName);
        Task<JsonResult> IsEmailExistAsync(string email);
        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationsAsync();
        ChallengeResult ConfigureExternalLogins(string provider, string controllerName, string actionName,
            string returnUrl);
        Task<bool> ConfigureExternalLoginsCallBacksGetUriByActionAsync(string remoteError = null);
        Task<LoginWithPasswordResult> LoginWithPasswordAsync(LoginViewModel loginViewModel); 
        Task<bool> LogOutUserAsync();
        Task<bool> SendRestPasswordLinkAsync(string userEmail, string returnController, string returnAction);
        Task<bool> RestPasswordAsync(string userEmail, string token, string newPassword);
        Task<bool> IsUserHasPasswordAsync(CustomIdentityUser user);
        Task<bool> IsUserHasPasswordByIdAsync(string userId);  
        Task<bool> IsLoggedUserHasPasswordAsync();
        Task<IdentityResult> EditAccountAsync(EditAccountViewModel editAccountViewModel); 

        Task<IEnumerable<UserViewModel>> GetUsersAsync();
        Task<UserDetailsViewModel> GetUserAsync(string userId);
        Task<int> GetUsersCountAsync();
        Task<EditUserViewModel> GetUserForEditAsync(string userId);
        Task<bool> DeleteUserAsync(string userId);
        Task<IEnumerable<SelectListItem>> GetRolesAsync();
        Task<IdentityResult> CreateUserByAdminAsync(CreateUserViewModel userViewModel);
        Task<IdentityResult> EditUserByAdminAsync(EditUserViewModel userViewModel);
        Task<bool> UpdateUserAsync(CustomIdentityUser loggedUser);
        Task<CustomIdentityUser> GetLoggedUserAsync();
        Task<CustomIdentityUser> GetLoggedUserWithFavoriteProductsAsync();
        Task<CustomIdentityUser> GetLoggedUserWithDetailsAsync();
        Task<bool> IsProductInLoggedUserFavoriteProductsAsync(int productId);
        Task<string> GetUserRoleAsync(CustomIdentityUser user);
        Task<string> GetLoggedUserRoleAsync();
    }
}
