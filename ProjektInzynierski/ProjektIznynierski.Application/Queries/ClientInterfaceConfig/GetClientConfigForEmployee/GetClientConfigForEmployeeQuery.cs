using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Queries.ClientInterfaceConfig.GetClientConfigForEmployee
{
    public class GetClientConfigForEmployeeQuery : IRequest<List<ClientInterfaceConfigDto>>
    {
        public ClientInterfacePlatform Platform { get; set; }
    }
}
