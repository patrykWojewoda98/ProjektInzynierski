using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Commands.ClientInterfaceConfig.CreateClientInterfaceConfig
{
    public class CreateClientInterfaceConfigCommand : IRequest<ClientInterfaceConfigDto>
    {
        public ClientInterfacePlatform Platform { get; set; }
        public string Key { get; set; }
        public string DisplayText { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int OrderIndex { get; set; }
        public bool IsVisible { get; set; }
        public int? ModifiedByEmployeeId { get; set; }
    }
}
