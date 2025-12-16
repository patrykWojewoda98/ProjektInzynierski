using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class RegionCodeConfiguration : BaseEntityConfiguration<RegionCode>
    {
        public override void Configure(EntityTypeBuilder<RegionCode> builder)
        {
            builder.ToTable("RegionCode");

            builder.Property(r => r.Code)
                   .HasMaxLength(4)
                   .IsRequired(true);

            

            base.Configure(builder);
        }
    }
}
