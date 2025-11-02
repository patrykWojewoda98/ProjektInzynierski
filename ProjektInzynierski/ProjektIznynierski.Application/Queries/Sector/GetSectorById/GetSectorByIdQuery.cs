using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Sector.GetSectorById
{
    public record GetSectorByIdQuery(int id) : IRequest<SectorDto>
    {
    }
}
