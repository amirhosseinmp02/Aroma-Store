using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Aroma_Shop.Domain.Models.MediaModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Aroma_Shop.Domain.Models.UserModels;
using Microsoft.AspNetCore.Identity;

namespace Aroma_Shop.Domain.Models.CustomIdentityModels
{
    public class CustomIdentityUser : IdentityUser
    {
        public CustomIdentityUser()
        {
            UserDetails = new UserDetails();
        }
        [Required]
        public DateTime RegisterTime { get; set; }  

        //Navigations Properties

        public UserDetails UserDetails { get; set; }
        public ICollection<Comment> UserComments { get; set; }
    }
}
