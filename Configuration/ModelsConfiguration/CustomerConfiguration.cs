using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoProjector.Models;

namespace VideoProjector.Configuration.ModelsConfiguration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Table name
            builder.ToTable("Customers");

            // Properties
            builder.Property(c => c.Address)
                .HasMaxLength(500); // Optional max length for address

            builder.Property(c => c.RegistrationDate)
                .IsRequired();

            builder.Property(c => c.IsDeleted)
                .IsRequired();

            builder.Property(c => c.ProfilePicture)
                .HasMaxLength(500); // Optional max length for profile picture URL

            builder.Property(c => c.Gender)
                .HasMaxLength(50); // Optional max length for gender
            
            // Relationships
            builder.HasMany(c => c.ShoppingCarts)
                .WithOne(sc => sc.Customer)
                .HasForeignKey(sc => sc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
