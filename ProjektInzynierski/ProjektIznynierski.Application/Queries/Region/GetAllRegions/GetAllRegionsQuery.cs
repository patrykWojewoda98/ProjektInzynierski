using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.Region.GetAllRegions
{
    public class GetAllRegionsQuery : IRequest<List<RegionDto>>
    {
    }
}
