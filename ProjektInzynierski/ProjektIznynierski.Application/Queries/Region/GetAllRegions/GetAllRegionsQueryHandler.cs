using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjektIznynierski.Application.Queries.Region.GetAllRegions
{
    public class GetAllRegionsQueryHandler : IRequestHandler<GetAllRegionsQuery, List<RegionDto>>
    {
        private readonly IRegionRepository _regionRepository;

        public GetAllRegionsQueryHandler(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public async Task<List<RegionDto>> Handle(GetAllRegionsQuery request, CancellationToken cancellationToken)
        {
            var regions = await _regionRepository.GetAllAsync(cancellationToken);
            
            return regions.Select(r => new RegionDto
            {
                Id = r.Id,
                Name = r.Name,
                RegionCodeId = r.RegionCodeId,
                RegionRisk = (int)r.RegionRisk
            }).ToList();
        }
    }
}
