using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface ITradeHistoryRepository : IRepository<TradeHistory>
    {
        Task<List<TradeHistory>> GetTradeHistoriesByWalletIdAsync(int walletId);
    }
}
