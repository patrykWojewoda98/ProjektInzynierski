using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.RegionCode.GetRegionCodeById
{
    internal class GetRegionCodeByIdQueryHandler: IRequestHandler<GetRegionCodeByIdQuery, RegionCodeDto>
    {
        private readonly IRegionCodeRepository _repository;

        public GetRegionCodeByIdQueryHandler(IRegionCodeRepository repository)
        {
            _repository = repository;
        }

        public async Task<RegionCodeDto> Handle( GetRegionCodeByIdQuery request,CancellationToken cancellationToken)
        {
            var regionCode = await _repository.GetByIdAsync(request.id, cancellationToken);

            if (regionCode is null)
            {
                throw new Exception($"RegionCode with id {request.id} not found.");
            }

            return new RegionCodeDto
            {
                Id = regionCode.Id,
                Code = regionCode.Code
            };
        }
    }
}
