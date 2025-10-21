using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public abstract class BaseEntityConfiguration<TBase> : IEntityTypeConfiguration<TBase>
        where TBase : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TBase> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedAt)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(e => e.UpdatedAt)
                .HasColumnType("datetime2")
                .IsRequired(false);
        }
    }
}
