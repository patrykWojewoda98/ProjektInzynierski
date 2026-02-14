using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Queries.ClientInterfaceConfig.GetClientMenuConfig
{
    public class GetClientMenuConfigQuery : IRequest<List<ClientMenuConfigItemDto>>
    {
        public ClientInterfacePlatform Platform { get; set; }
    }
}
