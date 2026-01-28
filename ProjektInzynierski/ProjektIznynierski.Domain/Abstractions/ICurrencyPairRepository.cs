using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface ICurrencyPairRepository : IRepository<CurrencyPair>
    {
        Task<CurrencyPair?> GetByCurrenciesAsync(int baseCurrencyId,int quoteCurrencyId);

        Task<List<CurrencyPair>> GetAllWithCurrenciesAsync();
    }
}
