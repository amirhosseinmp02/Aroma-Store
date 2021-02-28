using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Aroma_Shop.Domain.Models.CustomIdentityModels
{
    public class CustomIdentityRole : IdentityRole
    {
        public int Rank { get; set; }

        public CustomIdentityRole()
        {

        }
        public CustomIdentityRole(string roleName, int rank) : base(roleName)
        {
            Rank = rank;
        }
    }
}
