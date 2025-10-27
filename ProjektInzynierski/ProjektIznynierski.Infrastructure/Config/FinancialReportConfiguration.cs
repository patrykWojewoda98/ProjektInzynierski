using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class FinancialReportConfiguration : BaseEntityConfiguration<FinancialReport>
    {
        public override void Configure(EntityTypeBuilder<FinancialReport> builder)
        {
            builder.ToTable("FinancialReports");

            builder.Property(fr => fr.InvestInstrumentId)
                   .IsRequired();

            builder.Property(fr => fr.ReportDate)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.Property(fr => fr.Period)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(fr => fr.Revenue)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired(false);

            builder.Property(fr => fr.NetIncome)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired(false);

            builder.Property(fr => fr.EPS)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired(false);

            builder.Property(fr => fr.Assets)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired(false);

            builder.Property(fr => fr.Liabilities)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired(false);

            builder.Property(fr => fr.OperatingCashFlow)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired(false);

            builder.Property(fr => fr.FreeCashFlow)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired(false);

            builder.HasOne(fr => fr.InvestInstrument)
                   .WithMany(ii => ii.FinancialReports)
                   .HasForeignKey(fr => fr.InvestInstrumentId)
                   .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
