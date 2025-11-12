using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Sector.GetAllSectrors
{
    public record GetAllSectorsQuery : IRequest<List<SectorDto>>
    {
    }
}
