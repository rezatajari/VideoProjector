using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoProjector.Models;

namespace VideoProjector.Configuration.ModelsConfiguration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            // Table name
            builder.ToTable("OrderDetails");

            // Primary Key
            builder.HasKey(od => od.OrderDetailId);

            // Properties
            builder.Property(od => od.OrderDetailId)
                .ValueGeneratedOnAdd();

            builder.Property(od => od.OrderId)
                .IsRequired();

            builder.Property(od => od.ProductId)
                .IsRequired();

            builder.Property(od => od.Quantity)
                .IsRequired();

            builder.Property(od => od.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Relationships
            builder.HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
