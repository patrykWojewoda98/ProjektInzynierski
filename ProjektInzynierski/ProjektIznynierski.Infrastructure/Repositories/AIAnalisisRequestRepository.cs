using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class AIAnalisisRequestRepository : GenericRepository<AIAnalysisRequest>, IAIAnalisisRequestRepository
    {
        public AIAnalisisRequestRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
            
        }
        public Task<FinancialReport> GetFinancialReportByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<InvestProfile> GetInvestProfileByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }
}
