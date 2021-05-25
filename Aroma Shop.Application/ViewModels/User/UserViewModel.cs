using System;
using System.Collections.Generic;
using System.Text;

namespace Aroma_Shop.Application.ViewModels.User
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
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

        public DateTime RegisterTime { get; set; }      
    }
}
