using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Domain.Enums;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class ClientInterfaceConfigRepository : GenericRepository<ClientInterfaceConfig>, IClientInterfaceConfigRepository
    {
        public ClientInterfaceConfigRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<ClientInterfaceConfig>> GetByPlatformAndInterfaceTypeAsync(
            ClientInterfacePlatform platform,
            ClientInterfaceType interfaceType,
            bool visibleOnly,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<ClientInterfaceConfig>()
                .Where(c => c.Platform == platform && c.InterfaceType == interfaceType);

            if (visibleOnly)
                query = query.Where(c => c.IsVisible);

            return await query
                .OrderBy(c => c.OrderIndex)
                .ToListAsync(cancellationToken);
        }
    }
}
