using MediatR;

namespace ProjektIznynierski.Application.Commands.ClientInterfaceConfig.ReorderClientInterfaceConfig
{
    public class ReorderClientInterfaceConfigCommand : IRequest<Unit>
    {
        public List<ReorderItemDto> Items { get; set; } = new();
    }

    public class ReorderItemDto
    {
        public int Id { get; set; }
        public int OrderIndex { get; set; }
    }
}
