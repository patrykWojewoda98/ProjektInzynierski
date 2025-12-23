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
    }
}
