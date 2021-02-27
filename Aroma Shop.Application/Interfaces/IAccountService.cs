using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Aroma_Shop.Application.Interfaces
{
    public interface IAccountService
    {
        bool IsUserSignedIn(ClaimsPrincipal user);
    }
}
