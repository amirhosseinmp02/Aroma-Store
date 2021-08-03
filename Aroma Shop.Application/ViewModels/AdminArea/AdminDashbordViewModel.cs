using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.MediaModels;

namespace Aroma_Shop.Application.ViewModels.AdminArea
{
    public class AdminDashbordViewModel
    {
        public int UsersCount { get; set; }
        public int ProductsCount { get; set; }
        public int CompletedOrdersCount { get; set; }
        public int MessagesCount { get; set; }
        public int NumberOfVisits { get; set; } 
        public int UnCompletedOrdersCount { get; set; }
        public IEnumerable<Comment> Comments { get; set; }  
    }
}
