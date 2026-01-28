using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class ClientConfiguration : BaseEntityConfiguration<Client>
    {
        public override void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");

            builder.Property(c => c.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.HasIndex(c => c.Email)
                .IsUnique();

            builder.Property(c => c.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.PasswordHash)
            .IsRequired();

            builder.Property(c => c.City)
                .IsRequired(false);

            builder.Property(c => c.Address)
                .IsRequired(false);

            builder.Property(c => c.PostalCode)
                .IsRequired(false);

            builder.HasOne(c => c.Wallet)
                .WithOne(w => w.Client)
                .HasForeignKey<Wallet>(w => w.ClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.HasOne(c => c.InvestProfile)
                .WithOne(ip => ip.Client)
                .HasForeignKey<InvestProfile>(ip => ip.ClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.HasOne(c => c.Country)
                .WithMany(co => co.Clients)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.AIAnalysisResults)
                .WithOne(r => r.Client)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.WatchLists)
                .WithOne(wl => wl.Client)
                .HasForeignKey(wl => wl.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
