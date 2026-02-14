using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.ClientInterfaceConfig.UpdateClientInterfaceConfig
{
    public class UpdateClientInterfaceConfigCommand : IRequest<ClientInterfaceConfigDto>
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string DisplayText { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int OrderIndex { get; set; }
        public bool IsVisible { get; set; }
        public int? ModifiedByEmployeeId { get; set; }
    }
}
