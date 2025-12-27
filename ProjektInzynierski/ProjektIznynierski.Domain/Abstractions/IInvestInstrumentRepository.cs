using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IInvestInstrumentRepository : IRepository<InvestInstrument>
    {
        Task<List<InvestInstrument>> GetByRegionIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<InvestInstrument>> GetBySectorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<InvestInstrument>> GetByTypeIdAsync(int id, CancellationToken cancellationToken = default);
        Task<InvestInstrument?> GetByIsinAsync(string isin, CancellationToken ct);
    }
}
