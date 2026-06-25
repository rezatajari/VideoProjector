using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Models;

namespace API.Data.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(u => u.FullName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(u => u.Email)
               .HasMaxLength(150)
               .IsRequired();

        builder.HasIndex(u => u.Email)
               .IsUnique();

        builder.Property(u => u.PasswordHash)
               .IsRequired();

    }
}
