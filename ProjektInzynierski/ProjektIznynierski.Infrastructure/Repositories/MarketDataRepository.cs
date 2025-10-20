using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class MarketDataRepository : IMarketDataRepository
    {
        private readonly ProjektInzynierskiDbContext _dbContext;
        public MarketDataRepository(ProjektInzynierskiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<MarketData> GetByIdAsync(int id)
        {
            return await _dbContext.MarketDatas.SingleOrDefaultAsync(md => md.Id == id);
        }
        public void Add(MarketData entity)
        {
            _dbContext.MarketDatas.Add(entity);
        }
        public void Update(MarketData entity)
        {
            _dbContext.MarketDatas.Add(entity);
        }
        public void Delete(MarketData entity)
        {
            _dbContext.MarketDatas.Remove(entity);
        }
        
    }
}
