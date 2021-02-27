using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Aroma_Shop.Application.Interfaces;

namespace Aroma_Shop.Application.Services
{
    public class AccountService : IAccountService
    {
        public bool IsUserSignedIn(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }
    }
}
