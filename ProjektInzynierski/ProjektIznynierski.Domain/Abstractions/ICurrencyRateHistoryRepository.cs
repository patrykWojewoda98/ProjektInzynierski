using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface ICurrencyRateHistoryRepository: IRepository<CurrencyRateHistory>
    {
        Task<List<CurrencyRateHistory>> GetHistoryAsync(int currencyPairId);

        Task<CurrencyRateHistory?> GetLatestRateAsync(int currencyPairId);
    }
}
