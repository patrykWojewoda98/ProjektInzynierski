using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.RegionCode.GetAllRegionCodes
{
    public class GetAllRegionCodesQuery : IRequest<List<RegionCodeDto>>
    {
    }
}