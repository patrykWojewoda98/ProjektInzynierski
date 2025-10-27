using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class WatchListConfiguration : BaseEntityConfiguration<WatchList>
    {
        public override void Configure(EntityTypeBuilder<WatchList> builder)
        {
            builder.ToTable("WatchLists");

            builder.Property(wl => wl.ClientId)
                   .IsRequired();

            builder.Property(wl => wl.CreatedAt)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.HasOne(wl => wl.Client)
                   .WithMany(c => c.WatchLists)
                   .HasForeignKey(wl => wl.ClientId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(wl => wl.Items)
                   .WithOne(i => i.WatchList)
                   .HasForeignKey(i => i.WatchListId)
                   .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
