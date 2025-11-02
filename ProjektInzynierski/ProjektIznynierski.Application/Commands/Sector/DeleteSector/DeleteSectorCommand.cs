using MediatR;

namespace ProjektIznynierski.Application.Commands.Sector.DeleteSector
{
    public class DeleteSectorCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
