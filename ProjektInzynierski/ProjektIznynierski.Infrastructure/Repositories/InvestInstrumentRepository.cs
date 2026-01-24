using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class InvestInstrumentRepository : GenericRepository<InvestInstrument>, IInvestInstrumentRepository
    {
        public InvestInstrumentRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<InvestInstrument>> GetByRegionIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<InvestInstrument>()
                .Where(i => i.RegionId == id)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<InvestInstrument>> GetBySectorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<InvestInstrument>()
                .Where (i => i.SectorId == id)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<InvestInstrument>> GetByTypeIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<InvestInstrument>()
                .Where(i => i.InvestmentTypeId == id)
                .ToListAsync(cancellationToken);
        }

        public async Task<InvestInstrument?> GetByIsinAsync(string isin, CancellationToken ct)
        {
            return await _dbContext.Set<InvestInstrument>()
                .SingleOrDefaultAsync(i => i.Isin == isin, ct);
        }

        public async Task<InvestInstrument?> GetByTickerAsync(string Ticker, CancellationToken ct)
        {
            return await _dbContext.Set<InvestInstrument>()
                .SingleOrDefaultAsync(i => i.Ticker == Ticker, ct);
        }

        public Task<InvestInstrument> GetByFinacialMetricIdAsync(int id, CancellationToken ct)
        {
            return _dbContext.Set<InvestInstrument>()
                .Include(i => i.FinancialMetric)
                .Where(i => i.FinancialMetricId == id)
                .SingleAsync(ct);
        }
    }
}
