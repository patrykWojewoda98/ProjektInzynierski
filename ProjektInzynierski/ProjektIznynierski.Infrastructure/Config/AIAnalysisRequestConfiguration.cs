using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class AIAnalysisRequestConfiguration : BaseEntityConfiguration<AIAnalysisRequest>
    {
        public override void Configure(EntityTypeBuilder<AIAnalysisRequest> builder)
        {
            builder.ToTable("AIAnalysisRequests");

            builder.Property(r => r.FinancialReportId)
                   .IsRequired();

            builder.Property(r => r.InvestProfileId)
                   .IsRequired();

            builder.HasOne<FinancialReport>()
                   .WithMany()
                   .HasForeignKey(r => r.FinancialReportId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<InvestProfile>()
                   .WithMany()
                   .HasForeignKey(r => r.InvestProfileId)
                   .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
