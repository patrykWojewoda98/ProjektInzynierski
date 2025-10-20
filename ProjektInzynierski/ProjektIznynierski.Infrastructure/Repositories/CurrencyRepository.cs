
using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Domain.Enums;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class CurrencyRepository : ICurrencyRepository
    {
        private readonly ProjektInzynierskiDbContext _dbContext;

        public CurrencyRepository(ProjektInzynierskiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Currency> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Currencies.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
        public async Task<RiskLevel> GetRiscLevelById(int id, CancellationToken cancellationToken = default)
        {
            return this.GetByIdAsync(id, cancellationToken).Result.CurrencyRisk;
        }
        public void Add(Currency entity)
        {
            _dbContext.Currencies.Add(entity);
        }

        public void Update(Currency entity)
        {
            _dbContext.Currencies.Update(entity);
        }

        public void Delete(Currency entity)
        {
            _dbContext.Currencies.Remove(entity);
        }
        
    }
}
