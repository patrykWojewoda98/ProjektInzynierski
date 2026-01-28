using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Entities;


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
        public DbSet<FinancialMetric> FinancialMetrics { get; set; }
        public DbSet<InvestInstrument> InvestInstruments { get; set; }
        public DbSet<FinancialReport> FinancialReports { get; set; }
        public DbSet<MarketData> MarketDatas { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<TradeHistory> TradeHistories { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletInstrument> WalletInstruments { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }
        public DbSet<WatchListItem> WatchListItems { get; set; }
        public DbSet<InvestHorizon> InvestHorizons { get; set; }
        public DbSet<InvestmentType> InvestmentTypes { get; set; }
        public DbSet<RegionCode> RegionCodes { get; set; }
        public DbSet<RiskLevel> RiskLevels { get; set; }
        public DbSet<TradeType> TradeTypes { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public ProjektInzynierskiDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ProjektInzynierski");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjektInzynierskiDbContext).Assembly);
        }

    }
}
