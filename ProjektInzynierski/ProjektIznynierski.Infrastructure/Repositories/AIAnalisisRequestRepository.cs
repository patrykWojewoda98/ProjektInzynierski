using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    public class AIAnalisisRequestRepository : IAIAnalisisRequestRepository
    {
        public Task<FinancialReport> GetFinancialReportByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<InvestProfile> GetInvestProfileByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Add(AIAnalysisRequest request)
        {
            throw new NotImplementedException();
        }

        public void Update(AIAnalysisRequest request)
        {
            throw new NotImplementedException();
        }

        public void Delete(AIAnalysisRequest request)
        {
            throw new NotImplementedException();
        }
        
    }
}
