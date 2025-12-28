using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class WatchListItemRepository : GenericRepository<WatchListItem>, IWatchListItemRepository
    {
        public WatchListItemRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }


        public async Task<List<WatchListItem>> GetAllWatchListItemsByClientId(int clientId, CancellationToken cancellationToken)
        {
            return await _dbContext.WatchListItems.Where(x => x.Id == clientId).ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int watchListId, int investInstrumentId)
        {
            return await _dbContext.WatchListItems.AnyAsync(
                x => x.WatchListId == watchListId &&
                     x.InvestInstrumentId == investInstrumentId);
        }
    }
}
