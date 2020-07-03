using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart.Models;

// 2. Write Dbcontext
namespace ShoppingCart.Database
{
    public class ShoppingCartContext : DbContext
    {
        protected IConfiguration configuration;

        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>().HasKey(tbl => new { tbl.OrderId, tbl.ProductId });
            modelBuilder.Entity<CartDetail>().HasKey(tbl => new { tbl.CartId, tbl.ProductId });
            modelBuilder.Entity<Activation>().HasKey(tbl => new { tbl.ProductId, tbl.Code });
            modelBuilder.Entity<Review>().HasKey(tbl => new { tbl.UserId, tbl.ProductId, tbl.UtcDateTime });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Activation> Activations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
