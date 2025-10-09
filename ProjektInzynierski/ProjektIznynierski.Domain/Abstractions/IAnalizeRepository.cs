using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IAnalizeRepository
    {
        Task<Analize> GetAnalizeById(int id, CancellationToken cancellationToken = default);
    }
}
