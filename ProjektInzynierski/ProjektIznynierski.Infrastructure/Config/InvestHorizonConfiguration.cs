using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class InvestHorizonConfiguration : BaseEntityConfiguration<InvestHorizon>
    {
        public override void Configure(EntityTypeBuilder<InvestHorizon> builder)
        {
            builder.ToTable("InvestHorizon");

            builder.Property(i => i.Horizon)
                   .IsRequired();

           

            base.Configure(builder);
        }
    }
}
