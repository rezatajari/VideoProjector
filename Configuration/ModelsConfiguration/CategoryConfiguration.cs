using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoProjector.Models;

namespace VideoProjector.Configuration.ModelsConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Table name
            builder.ToTable("Categories");

            // Primary Key
            builder.HasKey(c => c.CategoryId);

            // Properties
            builder.Property(c => c.CategoryId)
                .ValueGeneratedOnAdd(); // Auto-increment for PK

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100); // Limit the length to 100 characters

            builder.Property(c => c.Description)
                .HasMaxLength(500); // Limit the length to 500 characters (optional)

            // Relationships
            builder.HasMany(c => c.Products) // One-to-Many relationship
                .WithOne(p => p.Category) // Assuming a Product has a navigation property `Category`
                .HasForeignKey(p => p.CategoryId) // FK in the `Products` table
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete (optional)
        }
    }
}
