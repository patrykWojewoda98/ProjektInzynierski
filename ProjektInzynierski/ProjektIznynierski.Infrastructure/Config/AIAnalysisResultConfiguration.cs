using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class AIAnalysisResultConfiguration : BaseEntityConfiguration<AIAnalysisResult>
    {
        public override void Configure(EntityTypeBuilder<AIAnalysisResult> builder)
        {
            builder.ToTable("AIAnalysisResults");

            builder.Property(r => r.Summary)
                   .HasMaxLength(2000)
                   .IsRequired(false);

            builder.Property(r => r.Recommendation)
                   .HasMaxLength(32)
                   .IsRequired(false);

            builder.Property(r => r.KeyInsights)
                   .HasMaxLength(4000)
                   .IsRequired(false);

            builder.Property(r => r.ConfidenceScore)
                   .HasColumnType("decimal(5,2)")
                   .IsRequired(false);

            builder.Property(r => r.ClientId)
                   .IsRequired();

            builder.HasOne(r => r.Client)
                   .WithMany(c => c.AIAnalysisResults)
                   .HasForeignKey(r => r.ClientId)
                   .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
