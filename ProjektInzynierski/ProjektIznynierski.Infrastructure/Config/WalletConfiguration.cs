using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class WalletConfiguration : BaseEntityConfiguration<Wallet>
    {
        public override void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets");

            builder.Property(w => w.CurrencyId)
                   .IsRequired();

            builder.Property(w => w.CashBalance)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasOne(w => w.Client)
                   .WithOne(c => c.Wallet)
                   .HasForeignKey<Wallet>(w => w.ClientId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired(false);

            builder.HasOne(w => w.Currency)
                   .WithMany()
                   .HasForeignKey(w => w.CurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(w => w.Instruments)
                   .WithOne(i => i.Wallet)
                   .HasForeignKey(i => i.WalletId)
                   .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
