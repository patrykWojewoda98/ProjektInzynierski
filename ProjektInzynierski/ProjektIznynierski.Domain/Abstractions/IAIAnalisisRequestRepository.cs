using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IAIAnalisisRequestRepository : IRepository<AIAnalysisRequest>
    {
        Task<FinancialReport> GetFinancialReportByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<InvestProfile> GetInvestProfileByIdAsync(int id, CancellationToken cancellationToken = default);
        
    }
}
