using MediatR;

namespace ProjektIznynierski.Application.Commands.ClientInterfaceConfig.DeleteClientInterfaceConfig
{
    public class DeleteClientInterfaceConfigCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
