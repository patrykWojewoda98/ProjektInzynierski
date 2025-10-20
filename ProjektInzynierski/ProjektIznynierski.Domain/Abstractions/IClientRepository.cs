using ProjektIznynierski.Domain.Entities;


namespace ProjektIznynierski.Domain.Abstractions
{
    public class IClientRepository
    {
        Task<AIAnalysisResult> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        void Add(Client entity);
        void Update(Client entity);
        void Delete(Client entity);
    }
}
