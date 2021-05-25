using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.CustomIdentityModels;

namespace Aroma_Shop.Application.ViewModels.User
{
    class UserDetailViewModel
    {
        public CustomIdentityUser User { get; set; }
        public string UserRole { get; set; }
    }
}
