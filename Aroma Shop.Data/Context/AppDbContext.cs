using Aroma_Shop.Domain.Models;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Aroma_Shop.Domain.Models.UserModels;
using Aroma_Shop.Domain.Models.MessageModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aroma_Shop.Data.Context
{
    public class AppDbContext : IdentityDbContext<CustomIdentityUser, CustomIdentityRole, string>
    {
        public AppDbContext(DbContextOptions options)
            :base(options)
        {
            
        }

        public DbSet<UserDetail> UsersDetails { get; set; }

        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageReply> MessagesReplies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductInformation> ProductsInformations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }
    }
}
