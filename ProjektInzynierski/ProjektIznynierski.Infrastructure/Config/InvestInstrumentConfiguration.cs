using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class InvestInstrumentConfiguration : BaseEntityConfiguration<InvestInstrument>
    {
        public override void Configure(EntityTypeBuilder<InvestInstrument> builder)
        {
            builder.ToTable("InvestInstruments");

            builder.Property(i => i.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.HasIndex(i => i.Name);

            builder.Property(i => i.Ticker)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(i => i.Type)
                   .IsRequired();

            builder.Property(i => i.Description)
                   .HasMaxLength(2000)
                   .IsRequired(false);

            builder.Property(i => i.MarketDataDate)
                   .HasColumnType("datetime2")
                   .IsRequired(false);

            builder.Property(i => i.SectorId).IsRequired();
            builder.Property(i => i.RegionId).IsRequired();
            builder.Property(i => i.CountryId).IsRequired();
            builder.Property(i => i.CurrencyId).IsRequired();
            builder.Property(i => i.FinancialMetricId).IsRequired(false);

            builder.HasOne(i => i.Sector)
                   .WithMany()
                   .HasForeignKey(i => i.SectorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Region)
                   .WithMany()
                   .HasForeignKey(i => i.RegionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Country)
                   .WithMany()
                   .HasForeignKey(i => i.CountryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Currency)
                   .WithMany()
                   .HasForeignKey(i => i.CurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.FinancialMetric)
                   .WithOne()
                   .HasForeignKey<InvestInstrument>(i => i.FinancialMetricId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.SetNull);

            base.Configure(builder);
        }
    }
}
