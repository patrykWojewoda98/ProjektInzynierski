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

            // 🔹 Name
            builder.Property(c => c.Name)
                .HasMaxLength(150)
                .IsRequired();

            // 🔹 Email (unikalny i wymagany)
            builder.HasIndex(c => c.Email)
                .IsUnique();

            builder.Property(c => c.Email)
                .HasMaxLength(255)
                .IsRequired();

            // 🔹 City, Address, PostalCode – brak limitu znaków
            builder.Property(c => c.City)
                .IsRequired(false); // może być null

            builder.Property(c => c.Address)
                .IsRequired(false);

            builder.Property(c => c.PostalCode)
                .IsRequired(false);

            // 🔹 Relacja z Wallet (1:1)
            builder.HasOne(c => c.Wallet)
                .WithOne(w => w.Client)
                .HasForeignKey<Client>(c => c.WalletID)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Relacja z InvestProfile (1:1)
            builder.HasOne(c => c.InvestProfile)
                .WithOne(ip => ip.Client)
                .HasForeignKey<Client>(c => c.InvestProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Relacja z Country (wiele klientów może należeć do jednego kraju)
            builder.HasOne(c => c.Country)
                .WithMany(co => co.Clients)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Relacja z AIAnalysisResults (1:N)
            builder.HasMany(c => c.AIAnalysisResults)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Relacja z WatchLists (1:N)
            builder.HasMany(c => c.WatchLists)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
}
