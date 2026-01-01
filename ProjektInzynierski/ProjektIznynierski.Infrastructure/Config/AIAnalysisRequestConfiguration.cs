using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class AIAnalysisRequestConfiguration : BaseEntityConfiguration<AIAnalysisRequest>
    {
        public override void Configure(EntityTypeBuilder<AIAnalysisRequest> builder)
        {
            base.Configure(builder);

            builder.ToTable("AIAnalysisRequests");

            builder.Property(x => x.InvestInstrumentId)
                   .IsRequired();

            builder.Property(x => x.ClientId)
                   .IsRequired();

            builder.Property(x => x.AIAnalysisResultId)
                   .IsRequired(false);

            builder.HasOne(x => x.AIAnalysisResult)
                   .WithMany()
                   .HasForeignKey(x => x.AIAnalysisResultId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
