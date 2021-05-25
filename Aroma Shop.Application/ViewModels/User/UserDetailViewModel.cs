using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Application.ViewModels.User
{
    public class UserDetailViewModel
    {
        public CustomIdentityUser User { get; set; }
        public string UserRole { get; set; }
    }
}
