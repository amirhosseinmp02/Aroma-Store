using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Aroma_Shop.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IAccountService
    {
        bool IsUserSignedIn(ClaimsPrincipal user);
        Task<IdentityResult> CreateUser(CustomIdentityUser user, string password);
        Task<bool> SendEmailConfirmation(CustomIdentityUser user,string controllerName,string actionName);
        Task<JsonResult> IsUserNameExist(string userName);
        Task<JsonResult> IsEmailExist(string email);
    }
}
