using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class SectorConfiguration : BaseEntityConfiguration<Sector>
    {
        public override void Configure(EntityTypeBuilder<Sector> builder)
        {
            builder.ToTable("Sectors");

            builder.Property(s => s.Name)
                   .HasMaxLength(120)
                   .IsRequired();

            builder.HasIndex(s => s.Name)
                   .IsUnique();

            builder.Property(s => s.Code)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasIndex(s => s.Code)
                   .IsUnique();

            builder.Property(s => s.Description)
                   .HasMaxLength(1000)
                   .IsRequired(false);

            base.Configure(builder);
        }
    }
}
