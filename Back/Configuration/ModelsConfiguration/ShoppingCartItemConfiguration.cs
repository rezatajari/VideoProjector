using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoProjector.Models;

namespace VideoProjector.Configuration.ModelsConfiguration
{
    public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            // Table name
            builder.ToTable("ShoppingCartItems");

            // Primary Key
            builder.HasKey(sci => sci.ShoppingCartItemId);

            // Properties
            builder.Property(sci => sci.ShoppingCartItemId)
                .ValueGeneratedOnAdd();

            builder.Property(sci => sci.ShoppingCartId)
                .IsRequired();

            builder.Property(sci => sci.ProductId)
                .IsRequired();

            builder.Property(sci => sci.Quantity)
                .IsRequired();

            builder.Property(sci => sci.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Relationships
            builder.HasOne(sci => sci.ShoppingCart)
                .WithMany(sc => sc.Items)
                .HasForeignKey(sci => sci.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sci => sci.Product)
                .WithMany() // No need for a collection in Product model for ShoppingCartItems
                .HasForeignKey(sci => sci.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // You can decide on delete behavior
        }
    }
}
