using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Region.GetRegionById
{
    public record GetRegionByIdQuery(int id) : IRequest<RegionDto>
    {
    }
}
