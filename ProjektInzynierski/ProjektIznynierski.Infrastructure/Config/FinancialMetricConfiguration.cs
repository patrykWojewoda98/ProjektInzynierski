using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class FinancialMetricConfiguration : BaseEntityConfiguration<FinancialMetric>
    {
        public override void Configure(EntityTypeBuilder<FinancialMetric> builder)
        {
            builder.ToTable("FinancialMetrics");

            builder.Property(f => f.PE)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired(false);

            builder.Property(f => f.PB)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired(false);

            builder.Property(f => f.ROE)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired(false);

            builder.Property(f => f.DebtToEquity)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired(false);

            builder.Property(f => f.DividendYield)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired(false);

            builder.HasOne(f => f.InvestmentInstrument)
       .WithOne(i => i.FinancialMetric)
       .HasForeignKey<FinancialMetric>(f => f.InvestmentInstrumentId);

            base.Configure(builder);
        }
    }
}
