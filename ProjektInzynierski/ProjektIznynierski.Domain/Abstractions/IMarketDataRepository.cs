using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IMarketDataRepository : IRepository<MarketData>
    {
        Task<MarketData?> GetLatestByInvestInstrumentIdAsync(int investInstrumentId, CancellationToken cancellationToken);
    }
}
