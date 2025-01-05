using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VideoProjector.Configuration.ModelsConfiguration;
using VideoProjector.Models;

namespace VideoProjector.Data
{
    public class VpDatabase(DbContextOptions<VpDatabase> options) : IdentityDbContext<Customer>(options)
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OerDetails { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply models configuration
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new CustomerConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new OrderDetailConfiguration());
            builder.ApplyConfiguration(new ShoppingCartConfiguration());
            builder.ApplyConfiguration(new ShoppingCartItemConfiguration());

        }
    }
}
