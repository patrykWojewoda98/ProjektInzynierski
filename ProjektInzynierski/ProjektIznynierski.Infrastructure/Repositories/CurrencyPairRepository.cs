using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class CurrencyPairRepository: GenericRepository<CurrencyPair>, ICurrencyPairRepository
    {
        public CurrencyPairRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<CurrencyPair?> GetByCurrenciesAsync(int baseCurrencyId,int quoteCurrencyId)
        {
            return await _dbContext.CurrencyPairs
                .Include(cp => cp.BaseCurrency)
                .Include(cp => cp.QuoteCurrency)
                .SingleOrDefaultAsync(cp =>
                    cp.BaseCurrencyId == baseCurrencyId &&
                    cp.QuoteCurrencyId == quoteCurrencyId);
        }

        public async Task<List<CurrencyPair>> GetAllWithCurrenciesAsync()
        {
            return await _dbContext.CurrencyPairs
                .Include(cp => cp.BaseCurrency)
                .Include(cp => cp.QuoteCurrency)
                .ToListAsync();
        }
    }
}
