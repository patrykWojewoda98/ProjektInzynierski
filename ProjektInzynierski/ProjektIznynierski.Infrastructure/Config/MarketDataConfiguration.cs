using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class MarketDataConfiguration : BaseEntityConfiguration<MarketData>
    {
        public override void Configure(EntityTypeBuilder<MarketData> builder)
        {
            builder.ToTable("MarketData");

            builder.Property(md => md.InvestInstrumentId)
                   .IsRequired();

            builder.Property(md => md.Date)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(md => md.OpenPrice)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired();

            builder.Property(md => md.ClosePrice)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired();

            builder.Property(md => md.HighPrice)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired();

            builder.Property(md => md.LowPrice)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired();

            builder.Property(md => md.Volume)
                   .IsRequired();

            builder.HasOne(md => md.InvestInstrument)
                   .WithMany(ii => ii.MarketData)
                   .HasForeignKey(md => md.InvestInstrumentId)
                   .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
