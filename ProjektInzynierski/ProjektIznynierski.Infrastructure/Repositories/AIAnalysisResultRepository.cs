using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class AIAnalysisResultRepository : IAIAnalysisResultRepository
    {
        private readonly ProjektInzynierskiDbContext _dbContext;
        public AIAnalysisResultRepository(ProjektInzynierskiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<AIAnalysisResult> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AIAnalysisResults.SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
        public void Add(AIAnalysisResult entity)
        {
            _dbContext.AIAnalysisResults.Add(entity);
        }

        public void Update(AIAnalysisResult entity)
        {
            _dbContext.AIAnalysisResults.Update(entity);
        }

        public void Delete(AIAnalysisResult entity)
        {
            _dbContext.AIAnalysisResults.Remove(entity);
        }
        
    }
}
