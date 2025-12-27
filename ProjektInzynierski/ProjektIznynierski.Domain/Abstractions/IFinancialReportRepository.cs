using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IFinancialReportRepository : IRepository<FinancialReport>
    {
        Task<List<FinancialReport>> GetByInstrumentIdAsync(int instrumentId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int investInstrumentId, string period, CancellationToken ct);
    }
}
