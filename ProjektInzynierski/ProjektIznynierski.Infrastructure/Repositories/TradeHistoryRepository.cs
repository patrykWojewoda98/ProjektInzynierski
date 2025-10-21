using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;


namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class TradeHistoryRepository : GenericRepository<TradeHistory>, ITradeHistoryRepository
    {
        public TradeHistoryRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<TradeHistory>> GetTradeHistoriesByWalletIdAsync(int walletId)
        {
            return await _dbContext.TradeHistories
                .Where(th => th.WalletId == walletId)
                .ToListAsync();
        }
    }
}
