using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class TradeHistoryConfiguration : BaseEntityConfiguration<TradeHistory>
    {
        public override void Configure(EntityTypeBuilder<TradeHistory> builder)
        {
            builder.ToTable("TradeHistories");

            builder.Property(th => th.WalletId)
                   .IsRequired();

            builder.Property(th => th.InvestInstrumentId)
                   .IsRequired();

            builder.Property(th => th.Quantity)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired();

            builder.Property(th => th.Price)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired();

            builder.Property(th => th.TradeTypeId)
                   .IsRequired();

            builder.Property(th => th.TradeDate)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.HasOne(th => th.Wallet)
                   .WithMany()
                   .HasForeignKey(th => th.WalletId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(th => th.InvestInstrument)
                   .WithMany()
                   .HasForeignKey(th => th.InvestInstrumentId)
                   .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
