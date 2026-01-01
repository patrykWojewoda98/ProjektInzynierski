using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IInvestProfileRepository : IRepository<InvestProfile>
    {
        Task<InvestProfile?> GetByClientIdAsync(int clientId,CancellationToken cancellationToken = default);
    }
}
