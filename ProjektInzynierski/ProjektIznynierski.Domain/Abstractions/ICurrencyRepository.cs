using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface ICurrencyRepository
    {
        Task<Currency> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<RiskLevel> GetRiscLevelById(int id,CancellationToken cancellationToken = default);
        void Add(Currency entity);
        void Update(Currency entity);
        void Delete(Currency entity);

    }
}
