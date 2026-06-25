using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Models;

namespace API.Data.Configurations;

public class ProductConfiguration : BaseEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Name)
               .HasMaxLength(150)
               .IsRequired();
        
        builder.Property(p => p.Category)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(p => p.Description)
               .HasMaxLength(1000);

        builder.Property(p => p.SalePrice)
               .HasColumnType("decimal(18,2)");

        builder.Property(p => p.QuantityForSale)
               .HasDefaultValue(0)
               .IsRequired();

        builder.Property(p => p.RentalPricePerDay)
               .HasColumnType("decimal(18,2)");

        builder.Property(p => p.QuantityForRental)
               .HasDefaultValue(0)
               .IsRequired();
    }
}