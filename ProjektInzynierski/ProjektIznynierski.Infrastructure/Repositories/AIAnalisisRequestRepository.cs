using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class AIAnalisisRequestRepository : IAIAnalisisRequestRepository
    {
        private readonly ProjektInzynierskiDbContext _dbContext;
        public AIAnalisisRequestRepository(ProjektInzynierskiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AIAnalysisRequest> GetAIAnalysisRequestByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AIAnalysisRequests.SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
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
            _dbContext.AIAnalysisRequests.Add(request);
        }

        public void Update(AIAnalysisRequest request)
        {
            _dbContext.AIAnalysisRequests.Update(request);
        }

        public void Delete(AIAnalysisRequest request)
        {
            _dbContext.AIAnalysisRequests.Remove(request);
        }

    }
}
