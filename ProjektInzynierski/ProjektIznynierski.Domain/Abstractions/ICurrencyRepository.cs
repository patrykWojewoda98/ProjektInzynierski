using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        Task<RiskLevel> GetRiscLevelById(int id,CancellationToken cancellationToken = default);


    }
}
