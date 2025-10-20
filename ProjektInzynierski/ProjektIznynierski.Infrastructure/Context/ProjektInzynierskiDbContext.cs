using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Infrastructure.Context
{
    internal class ProjektInzynierskiDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<AIAnalysisRequest> AIAnalysisRequests { get; set; }
        public DbSet<AIAnalysisResult> AIAnalysisResults { get; set; }
        public DbSet<InvestProfile> InvestProfiles { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public ProjektInzynierskiDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ProjektInzynierski");
        }

    }
}
