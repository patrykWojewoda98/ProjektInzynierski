using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class InvestProfileConfiguration : BaseEntityConfiguration<InvestProfile>
    {
        public override void Configure(EntityTypeBuilder<InvestProfile> builder)
        {
            builder.ToTable("InvestProfiles");

            builder.Property(ip => ip.ProfileName)
                   .HasMaxLength(120)
                   .IsRequired();

            builder.Property(ip => ip.AcceptableRisk)
                   .IsRequired();

            builder.Property(ip => ip.InvestHorizon)
                   .IsRequired();

            builder.Property(ip => ip.TargetReturn)
                   .IsRequired(false);

            builder.Property(ip => ip.MaxDrawDown)
                   .IsRequired(false);

            builder.Property(ip => ip.ClientId)
                   .IsRequired();

            builder.HasOne(ip => ip.Client)
                   .WithOne(c => c.InvestProfile)
                   .HasForeignKey<InvestProfile>(ip => ip.ClientId)
                   .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
