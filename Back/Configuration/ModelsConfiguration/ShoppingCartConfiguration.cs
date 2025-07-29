using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoProjector.Models;

namespace VideoProjector.Configuration.ModelsConfiguration
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            // Table name
            builder.ToTable("ShoppingCarts");

            // Primary Key
            builder.HasKey(sc => sc.ShoppingCartId);

            // Properties
            builder.Property(sc => sc.ShoppingCartId)
                .ValueGeneratedOnAdd();

            builder.Property(sc => sc.CustomerId)
                .IsRequired();

            builder.Property(sc => sc.CreatedDate)
                .IsRequired();

            // Relationships
            builder.HasOne(sc => sc.Customer)
                .WithMany(c => c.ShoppingCarts)
                .HasForeignKey(sc => sc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sc => sc.Items)
                .WithOne(item => item.ShoppingCart)
                .HasForeignKey(item => item.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
