using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        Task<RiskLevel> GetRiscLevelById(int id,CancellationToken cancellationToken = default);

        Task<List<Currency>> GetAllAsync(CancellationToken cancellationToken = default);

    }
}
