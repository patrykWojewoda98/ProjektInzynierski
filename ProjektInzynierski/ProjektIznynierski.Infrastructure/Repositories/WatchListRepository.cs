using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class WatchListRepository : GenericRepository<WatchList>, IWatchListRepository
    {
        public WatchListRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<WatchListItem>> GetWatchListItemsByClientIdAsync(int clientId)
        {
            return await _dbContext.WatchListItems
                .Where(wli => wli.WatchList.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<List<WatchListItem>> GetWatchListItemsByWatchListIdAsync(int watchListId)
        {
            return await _dbContext.WatchListItems
                .Where(wli => wli.WatchListId == watchListId)
                .ToListAsync();
        }
    }
}
