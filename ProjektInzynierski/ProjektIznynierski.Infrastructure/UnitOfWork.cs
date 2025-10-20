using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Infrastructure
{
    internal class UnitOfWork : IUnitOfWork
    {
        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
