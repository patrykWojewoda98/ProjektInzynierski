using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IWatchListItemRepository : IRepository<WatchListItem>
    {
        Task<List<WatchListItem>> GetAllWatchListItemsByClientId(int clientId, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(int watchListId, int investInstrumentId);
    }
}
