using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class CountryConfiguration : BaseEntityConfiguration<Country>
    {
        public override void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");

            builder.Property(c => c.Name)
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(c => c.IsoCode)
                   .HasMaxLength(3)
                   .IsRequired();

            builder.HasIndex(c => c.IsoCode)
                   .IsUnique();

            builder.Property(c => c.CountryRiskLevelId)
                   .IsRequired();

            builder.Property(c => c.RegionId)
                   .IsRequired();

            builder.Property(c => c.CurrencyId)
                   .IsRequired();

            builder.HasOne(c => c.Region)
                   .WithMany(r => r.Countries)
                   .HasForeignKey(c => c.RegionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Currency)
                   .WithMany()
                   .HasForeignKey(c => c.CurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
