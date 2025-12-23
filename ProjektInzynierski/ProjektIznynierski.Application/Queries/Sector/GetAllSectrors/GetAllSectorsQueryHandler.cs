using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Sector.GetAllSectrors
{
    internal class GetAllSectorsQueryHandler : IRequestHandler<GetAllSectorsQuery, List<SectorDto>>
    {
        private readonly ISectorRepository _sectorRepository;
        public GetAllSectorsQueryHandler(ISectorRepository sectorRepository)
        {
            _sectorRepository = sectorRepository;
        }
        public async Task<List<SectorDto>> Handle(GetAllSectorsQuery request, CancellationToken cancellationToken)
        {
            var sectors = await _sectorRepository.GetAllAsync();
            return sectors.Select(sector => new SectorDto
            {
                Id = sector.Id,
                Code = sector.Code,
                Name = sector.Name,
                Description = sector.Description
            }).ToList();
        }
    }
}
