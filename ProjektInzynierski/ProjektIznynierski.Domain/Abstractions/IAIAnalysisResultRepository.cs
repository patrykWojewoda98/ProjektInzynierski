using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IAIAnalysisResultRepository
    {
        Task<AIAnalysisResult> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        void Add(AIAnalysisResult entity);
        void Update(AIAnalysisResult entity);
        void Delete(AIAnalysisResult entity);
    }
}
