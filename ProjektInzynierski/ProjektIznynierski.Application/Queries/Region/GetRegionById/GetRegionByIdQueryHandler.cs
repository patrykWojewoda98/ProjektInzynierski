using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Region.GetRegionById
{
    internal class GetRegionByIdQueryHandler : IRequestHandler<GetRegionByIdQuery, RegionDto>
    {
        private readonly IRegionRepository _repository;
        public GetRegionByIdQueryHandler(IRegionRepository repository)
        {
            _repository = repository;
        }

        public async Task<RegionDto> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"Region with id {request.id} not found.");
            }

            return new RegionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                RegionCodeId = entity.RegionCodeId,
                RegionRiskLevelId = entity.RegionRiskLevelId
            };
        }
    }
}
