using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class RegionConfiguration : BaseEntityConfiguration<Region>
    {
        public override void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.ToTable("Regions");

            builder.Property(r => r.Name)
                   .HasMaxLength(120)
                   .IsRequired();

            builder.HasIndex(r => r.Name)
                   .IsUnique();

            builder.Property(r => r.Code)
                   .IsRequired();

            builder.Property(r => r.RegionRisk)
                   .IsRequired();

            base.Configure(builder);
        }
    }
}
