using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository
    {

        public CurrencyRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<RiskLevel> GetRiscLevelById(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }
}
