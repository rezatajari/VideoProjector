using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Models;

namespace API.Data.Configurations;

public class OrderConfiguration : BaseEntityConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.Property(o => o.TotalPrice)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(o => o.Quantity)
               .IsRequired();

        builder.Property(o => o.IsRental)
               .IsRequired();

        builder.Property(o => o.Status)
               .HasDefaultValue(OrderStatus.Pending)
               .IsRequired();

        builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(o => o.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
