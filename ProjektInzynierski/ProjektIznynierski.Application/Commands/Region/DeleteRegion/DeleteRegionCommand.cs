using MediatR;

namespace ProjektIznynierski.Application.Commands.Region.DeleteRegion
{
    public class DeleteRegionCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
