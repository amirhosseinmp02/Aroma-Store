using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Application.ViewModels.User
{
    public class UserDetailsViewModel
    {
        public CustomIdentityUser User { get; set; }
        public string UserRoleName { get; set; }
        public string PersianUserRoleName
        {
            get
            {
                if (UserRoleName == "Founder")
                {
                    return "موسس";
                }
                else if (UserRoleName == "Manager")
                {
                    return "مدیر";
                }
                else if (UserRoleName == "Writer")
                {
                    return "نویسنده";
                }
                else
                {
                    return "مشتری";
                }
            }
        }
    }
}
