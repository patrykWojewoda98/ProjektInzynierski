using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class InvestmentTypeConfiguration : BaseEntityConfiguration<InvestmentType>
    {
        public override void Configure(EntityTypeBuilder<InvestmentType> builder)
        {
            builder.ToTable("InvestmentType");

            builder.Property(i => i.TypeName)
                   .IsRequired();

            base.Configure(builder);
        }
    }
}
