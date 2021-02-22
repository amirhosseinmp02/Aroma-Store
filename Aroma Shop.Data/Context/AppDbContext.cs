using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aroma_Shop.Data.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options)
            :base(options)
        {
            
        }
    }
}
