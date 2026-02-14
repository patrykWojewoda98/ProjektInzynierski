using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class ClientInterfaceConfigConfiguration : BaseEntityConfiguration<ClientInterfaceConfig>
    {
        public override void Configure(EntityTypeBuilder<ClientInterfaceConfig> builder)
        {
            builder.ToTable("ClientInterfaceConfigs");

            builder.Property(c => c.Key)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(c => c.DisplayText)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(c => c.Description)
                .HasMaxLength(500);
            builder.Property(c => c.ImagePath)
                .HasMaxLength(500);

            builder.HasIndex(c => new { c.Platform, c.InterfaceType, c.Key })
                .IsUnique();

            builder.HasOne(c => c.ModifiedByEmployee)
                .WithMany()
                .HasForeignKey(c => c.ModifiedByEmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            base.Configure(builder);
        }
    }
}
