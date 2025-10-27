using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class WalletInstrumentConfiguration : BaseEntityConfiguration<WalletInstrument>
    {
        public override void Configure(EntityTypeBuilder<WalletInstrument> builder)
        {
            builder.ToTable("WalletInstruments");

            builder.Property(wi => wi.WalletId)
                   .IsRequired();

            builder.Property(wi => wi.InvestInstrumentId)
                   .IsRequired();

            builder.Property(wi => wi.Quantity)
                   .HasColumnType("decimal(18,4)")
                   .IsRequired();

            builder.HasOne(wi => wi.Wallet)
                   .WithMany(w => w.Instruments)
                   .HasForeignKey(wi => wi.WalletId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(wi => wi.InvestInstrument)
                   .WithMany()
                   .HasForeignKey(wi => wi.InvestInstrumentId)
                   .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
