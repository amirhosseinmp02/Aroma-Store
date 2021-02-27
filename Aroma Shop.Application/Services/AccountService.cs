using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Dynamic;
using System.Threading.Tasks;

namespace Aroma_Shop.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly SignInManager<CustomIdentityUser> _signInManager;

        public async Task<IdentityResult> CreateUser(CustomIdentityUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public bool IsUserSignedIn(ClaimsPrincipal user)
        {
            if (_signInManager.IsSignedIn(user))
                return true;
            return false;
        }
    }
}
