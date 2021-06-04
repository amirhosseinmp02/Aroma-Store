using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models.UserModels;

namespace Aroma_Shop.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public void DeleteUserDetails(UserDetails userDetail)
        {
            _context.Remove(userDetail);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
