using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IClientInterfaceConfigRepository : IRepository<ClientInterfaceConfig>
    {
        Task<List<ClientInterfaceConfig>> GetByPlatformAndInterfaceTypeAsync(
            ClientInterfacePlatform platform,
            ClientInterfaceType interfaceType,
            bool visibleOnly,
            CancellationToken cancellationToken = default);
    }
}
