using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class FinancialReportRepository : GenericRepository<FinancialReport>, IFinancialReportRepository
    {
        public FinancialReportRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {

        }

        public Task<List<FinancialReport>> GetByInstrumentIdAsync(int instrumentId, CancellationToken cancellationToken = default)
        {
            return _dbContext.Set<FinancialReport>()
                .Where(fr => fr.InvestInstrumentId == instrumentId)
                .ToListAsync(cancellationToken);
        }
    }
}
