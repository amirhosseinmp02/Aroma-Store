using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Application.ViewModels.Account;
using Aroma_Shop.Application.ViewModels.User;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IAccountService
    {
        bool IsUserSignedIn();
        Task<IdentityResult> CreateUser(CustomIdentityUser user, string password);
        Task<bool> SendEmailConfirmation(CustomIdentityUser user, string controllerName, string actionName);
        Task<bool> EmailConfirmation(string email, string token);
        Task<JsonResult> IsUserNameExist(string userName);
        Task<JsonResult> IsEmailExist(string email);
        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthentications();

        ChallengeResult ConfigureExternalLogins(string provider, string controllerName, string actionName,
            string returnUrl);
        Task<bool> ConfigureExternalLoginsCallBacks(string remoteError = null);
        Task<bool> LoginWithPassword(LoginViewModel vm);
        Task<bool> LogOutUser();
        Task<bool> SendRestPasswordLink(string userEmail, string returnController, string returnAction);
        Task<bool> RestPassword(string userEmail, string token, string newPassword);
        Task<IEnumerable<UserViewModel>> GetUsers();
        Task<UserDetailsViewModel> GetUser(string userId);
        Task<EditUserViewModel> GetUserForEdit(string userId);
        Task<bool> DeleteUser(string userId);
        Task<IEnumerable<SelectListItem>> GetRoles();
        Task<IdentityResult> CreateUserByAdmin(CreateUserViewModel userViewModel);
        Task<IdentityResult> EditUserByAdmin(EditUserViewModel userViewModel);
        Task<CustomIdentityUser> GetLoggedUser();
        Task<CustomIdentityUser> GetLoggedUserWithDetails();
        Task<string> GetLoggedUserRole();
    }
}
