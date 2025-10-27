using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class CurrencyConfiguration : BaseEntityConfiguration<Currency>
    {
        public override void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currencies");

            builder.Property(c => c.Name)
                   .HasMaxLength(80)
                   .IsRequired();

            builder.HasIndex(c => c.Name)
                   .IsUnique();

            builder.Property(c => c.CurrencyRisk)
                   .IsRequired();

            base.Configure(builder);
        }
    }
}
