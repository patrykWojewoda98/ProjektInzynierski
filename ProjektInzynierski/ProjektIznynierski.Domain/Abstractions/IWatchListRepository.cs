using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IWatchListRepository : IRepository<WatchList>
    {
        Task<List<WatchListItem>> GetWatchListItemsByWatchListIdAsync(int watchListId);
        Task<List<WatchListItem>> GetWatchListItemsByClientIdAsync(int clientId);
    }
}
