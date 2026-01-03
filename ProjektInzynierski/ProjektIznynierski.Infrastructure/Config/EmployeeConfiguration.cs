using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Config
{
    public class EmployeeConfiguration : BaseEntityConfiguration<Employee>
    {
        public override void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.Property(e => e.Name)
                .IsRequired();

            builder.HasIndex(e => e.Email)
                .IsUnique();

            builder.Property(e => e.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(e => e.Pesel)
                .IsUnique();

            builder.Property(e => e.Pesel)
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(e => e.PasswordHash)
                .IsRequired();

            builder.Property(e => e.IsAdmin)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(e => e.TwoFactorCodeHash)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(e => e.TwoFactorCodeExpiresAt)
                .IsRequired(false);

            base.Configure(builder);
        }
    }
}
