using ProjektIznynierski.Domain.Models;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IYahooFinanceService
    {
        Task<MarketDataSnapshot> GetMarketDataByTIcker (string ticker, CancellationToken ct);
    }
}
