using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class RiskLevelConfiguration : BaseEntityConfiguration<RiskLevel>
    {
        public override void Configure(EntityTypeBuilder<RiskLevel> builder)
        {
            builder.ToTable("RiskLevel");

            builder.Property(r => r.RiskLevelScale)
                   .IsRequired(true);

            builder.Property(r => r.Description)
                   .HasMaxLength(100)
                   .IsRequired(true);

            base.Configure(builder);
        }
    }
}
