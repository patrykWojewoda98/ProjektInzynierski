using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IAIAnalisisRequestRepository : IRepository<AIAnalysisRequest>
    {
        Task<List<AIAnalysisRequest>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default);

        Task<AIAnalysisRequest?> GetPendingByInstrumentAndClientAsync(int investInstrumentId,int clientId,CancellationToken cancellationToken = default);
    }
}
