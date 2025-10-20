using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class AIAnalysisResultRepository : IAIAnalysisResultRepository
    {
        public Task<AIAnalysisResult> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public void Add(AIAnalysisResult entity)
        {
            throw new NotImplementedException();
        }

        public void Update(AIAnalysisResult entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(AIAnalysisResult entity)
        {
            throw new NotImplementedException();
        }
        
    }
}
