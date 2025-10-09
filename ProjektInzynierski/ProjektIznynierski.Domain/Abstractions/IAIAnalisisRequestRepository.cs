using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IAIAnalisisRequestRepository
    {
        Task<FinancialReport> GetFinancialReportByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<InvestProfile> GetInvestProfileByIdAsync(int id, CancellationToken cancellationToken = default);
        void Add(AIAnalysisRequest request);
        void Update(AIAnalysisRequest request);
        void Delete(AIAnalysisRequest request);
    }
}
