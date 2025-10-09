using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    public class AnalizeRepository : IAnalizeRepository
    {
        public Task<Analize> GetAnalizeById(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
