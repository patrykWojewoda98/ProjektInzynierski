using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Domain.Enums;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository
    {

        public CurrencyRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Currency>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return this.GetAllAsync(cancellationToken);
        }

        public async Task<RiskLevel> GetRiscLevelById(int id, CancellationToken cancellationToken = default)
        {
            return this.GetByIdAsync(id, cancellationToken).Result.CurrencyRisk;
        }

    }
}
