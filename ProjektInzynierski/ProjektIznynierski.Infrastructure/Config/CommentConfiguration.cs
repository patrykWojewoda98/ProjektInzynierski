using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class CommentConfiguration : BaseEntityConfiguration<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.Property(c => c.Content)
                .IsRequired();

            builder.HasOne(c => c.Client)
            .WithMany(c => c.Comments)
            .HasForeignKey(c => c.ClientID)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.InvestInstrument)
                .WithMany(i => i.Comments)
                .HasForeignKey(c => c.InvestInstrumentID)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
