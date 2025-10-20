using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IInvestProfileRepository
    {
        Task<InvestProfile> GetInvestProfileByIdAsync(int id, CancellationToken cancellationToken = default);
        void Add(InvestProfile entity);
        void Update(InvestProfile entity);
        void Delete(InvestProfile entity);
    }
}
