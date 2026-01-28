using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class CurrencyRateHistoryConfiguration : BaseEntityConfiguration<CurrencyRateHistory>
    {
        public override void Configure(
            EntityTypeBuilder<CurrencyRateHistory> builder
        )
        {
            builder.ToTable("CurrencyRateHistories");

            builder.Property(rh => rh.Date)
                .IsRequired();

            builder.Property(rh => rh.CloseRate)
                .HasPrecision(18, 6)
                .IsRequired();

            builder.Property(rh => rh.OpenRate)
                .HasPrecision(18, 6)
                .IsRequired(false);

            builder.Property(rh => rh.HighRate)
                .HasPrecision(18, 6)
                .IsRequired(false);

            builder.Property(rh => rh.LowRate)
                .HasPrecision(18, 6)
                .IsRequired(false);

            builder.HasOne(rh => rh.CurrencyPair)
                .WithMany(cp => cp.RateHistory)
                .HasForeignKey(rh => rh.CurrencyPairId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasIndex(rh =>
                new { rh.CurrencyPairId, rh.Date }
            ).IsUnique();

            base.Configure(builder);
        }
    }
}
