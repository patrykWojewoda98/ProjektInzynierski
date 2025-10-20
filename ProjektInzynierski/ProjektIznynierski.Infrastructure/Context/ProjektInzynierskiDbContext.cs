using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Context
{
    internal class ProjektInzynierskiDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public ProjektInzynierskiDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ProjektInzynierski");
        }

    }
}
