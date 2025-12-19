using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class TradeTypeConfiguration : BaseEntityConfiguration<TradeType>
    {
        public override void Configure(EntityTypeBuilder<TradeType> builder)
        {
            builder.ToTable("TradeType");

            builder.Property(i => i.TradeTypeName)
                   .IsRequired();

            base.Configure(builder);
        }
    }
}
