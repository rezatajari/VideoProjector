using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Models;

namespace API.Data.Configurations;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedAt)
               .HasDefaultValueSql("GETDATE()")
               .IsRequired();

        builder.Property(e => e.IsDeleted)
               .HasDefaultValue(false)
               .IsRequired();
    }
}