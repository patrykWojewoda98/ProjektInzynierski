using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.RegionCode.GetRegionCodeById
{
    public record GetRegionCodeByIdQuery(int id) : IRequest<RegionCodeDto>
    {
    }
}