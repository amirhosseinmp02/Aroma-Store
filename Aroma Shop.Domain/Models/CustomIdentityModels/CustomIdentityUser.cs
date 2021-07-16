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
            FavoriteProducts = new List<Product>();
        }

        [Required]
        public DateTime RegisterTime { get; set; }  

        //Navigations Properties

        public UserDetails UserDetails { get; set; }
        public ICollection<Comment> UserComments { get; set; }
        public ICollection<Product> FavoriteProducts { get; set; }
        public ICollection<Order> UserOrders { get; set; }      
    }
}
