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

            builder.Property(ip => ip.AcceptableRiskLevelId)
                   .IsRequired();

            builder.HasOne(ip => ip.InvestHorizon)
            .WithMany()
            .HasForeignKey(ip => ip.InvestHorizonId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

            builder.Property(ip => ip.TargetReturn)
                   .IsRequired(false);

            builder.Property(ip => ip.MaxDrawDown)
                   .IsRequired(false);

            // 🔹 Relacja 1:1 z Client – klucz obcy w InvestProfile
            builder.HasOne(ip => ip.Client)
                   .WithOne(c => c.InvestProfile)
                   .HasForeignKey<InvestProfile>(ip => ip.ClientId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired(false);

            // 🔹 Many-to-many: InvestProfile ↔ Regions
            builder.HasMany(ip => ip.PreferredRegions)
                   .WithMany()
                   .UsingEntity(j => j.ToTable("InvestProfileRegions"));

            // 🔹 Many-to-many: InvestProfile ↔ Sectors
            builder.HasMany(ip => ip.PreferredSectors)
                   .WithMany()
                   .UsingEntity(j => j.ToTable("InvestProfileSectors"));

            base.Configure(builder);
        }
    }
}
