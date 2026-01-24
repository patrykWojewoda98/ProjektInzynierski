using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IFinancialMetricRepository : IRepository<FinancialMetric>
    {
        Task<FinancialMetric?> GetByInvestInstrumentIdAsync(int investInstrumentId, CancellationToken cancellationToken = default);
    }
}
