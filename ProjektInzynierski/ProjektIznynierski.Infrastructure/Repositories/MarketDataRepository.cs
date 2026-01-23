using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class MarketDataRepository : GenericRepository<MarketData>, IMarketDataRepository
    {
        
        public MarketDataRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> ExistsAsync(int investInstrumentId,DateTime date, CancellationToken ct)
        {
            return await _dbContext.MarketDatas.AnyAsync(
                md =>
                    md.InvestInstrumentId == investInstrumentId &&
                    md.Date.Date == date.Date,
                ct);
        }

        public async Task<List<MarketData>> GetByInvestInstrumentIdAsync(int investInstrumentId, CancellationToken cancellationToken)
        {
            return await _dbContext.MarketDatas
                .Where(md => md.InvestInstrumentId == investInstrumentId)
                .OrderBy(md => md.Date)
                .ToListAsync();
        }

        public async Task<MarketData?> GetLatestByInvestInstrumentIdAsync(int investInstrumentId, CancellationToken cancellationToken)
        {
            return await _dbContext.MarketDatas
                .Where(md => md.InvestInstrumentId == investInstrumentId)
                .OrderByDescending(md => md.Date)
                .FirstOrDefaultAsync();
        }
    }
}
