using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IMarketDataRepository
    {
        Task<MarketData> GetByIdAsync(int id);
        void Add(MarketData entity);
        void Update(MarketData entity);
        void Delete(MarketData entity);
    }
}
