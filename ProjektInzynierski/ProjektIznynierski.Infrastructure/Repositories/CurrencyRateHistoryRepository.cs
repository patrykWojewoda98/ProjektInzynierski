using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class CurrencyRateHistoryRepository: GenericRepository<CurrencyRateHistory>,ICurrencyRateHistoryRepository
    {
        public CurrencyRateHistoryRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<CurrencyRateHistory>> GetHistoryAsync(int currencyPairId)
        {
            return await _dbContext.CurrencyRateHistories
                .Where(h => h.CurrencyPairId == currencyPairId)
                .OrderByDescending(h => h.Date)
                .ToListAsync();
        }

        public async Task<CurrencyRateHistory?> GetLatestRateAsync(int currencyPairId)
        {
            return await _dbContext.CurrencyRateHistories
                .Where(h => h.CurrencyPairId == currencyPairId)
                .OrderByDescending(h => h.Date)
                .FirstOrDefaultAsync();
        }
    }
}
