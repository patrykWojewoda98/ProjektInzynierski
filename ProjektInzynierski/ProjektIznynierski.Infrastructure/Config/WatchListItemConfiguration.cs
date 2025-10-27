using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class WatchListItemConfiguration : BaseEntityConfiguration<WatchListItem>
    {
        public override void Configure(EntityTypeBuilder<WatchListItem> builder)
        {
            builder.ToTable("WatchListItems");

            builder.Property(wli => wli.WatchListId)
                   .IsRequired();

            builder.Property(wli => wli.InvestInstrumentId)
                   .IsRequired();

            builder.Property(wli => wli.AddedAt)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.HasOne(wli => wli.WatchList)
                   .WithMany(wl => wl.Items)
                   .HasForeignKey(wli => wli.WatchListId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(wli => wli.InvestInstrument)
                   .WithMany()
                   .HasForeignKey(wli => wli.InvestInstrumentId)
                   .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
