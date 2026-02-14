using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.ClientInterfaceConfig.ToggleVisibilityClientInterfaceConfig
{
    public class ToggleVisibilityClientInterfaceConfigCommand : IRequest<ClientInterfaceConfigDto>
    {
        public int Id { get; set; }
    }
}
