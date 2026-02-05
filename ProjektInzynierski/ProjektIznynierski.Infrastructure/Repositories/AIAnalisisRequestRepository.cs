using Microsoft.EntityFrameworkCore;
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

        public async Task<AIAnalysisRequest?> GetByIdAsync(int id,CancellationToken cancellationToken = default)
        {
            return await _dbContext.AIAnalysisRequests
                .Include(x => x.AIAnalysisResult)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<AIAnalysisRequest>> GetByClientIdAsync(int clientId,CancellationToken cancellationToken = default)
        {
            return await _dbContext.AIAnalysisRequests.Where(x => x.ClientId == clientId).ToListAsync(cancellationToken);
        }

        public async Task<AIAnalysisRequest?> GetPendingByInstrumentAndClientAsync(int investInstrumentId,int clientId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AIAnalysisRequests
                .Where(x =>
                    x.ClientId == clientId &&
                    x.InvestInstrumentId == investInstrumentId &&
                    x.AIAnalysisResultId == null)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

    }
}
