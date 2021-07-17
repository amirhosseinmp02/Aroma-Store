using System;
using Aroma_Shop.Domain.Models;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Aroma_Shop.Domain.Models.FileModels;
using Aroma_Shop.Domain.Models.MediaModels;
using Aroma_Shop.Domain.Models.PageModels;
using Aroma_Shop.Domain.Models.ProductModels;
using Aroma_Shop.Domain.Models.UserModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aroma_Shop.Data.Context
{
    public class AppDbContext : IdentityDbContext<CustomIdentityUser, CustomIdentityRole, string>
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<UserDetails> UsersDetails { get; set; }

        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageReply> MessagesReplies { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Page> Pages { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariation> ProductVariations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductInformation> ProductsInformations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrdersDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
                .Property(p => p.ProductAttributesNames)
                .HasConversion(
                    p => string.Join(',', p),
                    p => p.Split(",", StringSplitOptions.RemoveEmptyEntries));

            builder.Entity<Product>()
                .Property(p => p.ProductAttributesValues)
                .HasConversion(
                    p => string.Join(';', p),
                    p => p.Split(";", StringSplitOptions.RemoveEmptyEntries));

            builder.Entity<ProductVariation>()
                .Property(p => p.ProductVariationValues)
                .HasConversion(
                    p => string.Join(',', p),
                    p => p.Split(",", StringSplitOptions.RemoveEmptyEntries));

            base.OnModelCreating(builder);
        }
    }
}
