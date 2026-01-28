using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class CurrencyPairConfiguration : BaseEntityConfiguration<CurrencyPair>
    {
        public override void Configure(EntityTypeBuilder<CurrencyPair> builder)
        {
            builder.ToTable("CurrencyPairs");

            builder.Property(cp => cp.Symbol)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(cp =>
                new { cp.BaseCurrencyId, cp.QuoteCurrencyId }
            ).IsUnique();

            builder.HasOne(cp => cp.BaseCurrency)
                .WithMany()
                .HasForeignKey(cp => cp.BaseCurrencyId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(cp => cp.QuoteCurrency)
                .WithMany()
                .HasForeignKey(cp => cp.QuoteCurrencyId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasMany(cp => cp.RateHistory)
                .WithOne(rh => rh.CurrencyPair)
                .HasForeignKey(rh => rh.CurrencyPairId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
