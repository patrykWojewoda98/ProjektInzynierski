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
    }
}
