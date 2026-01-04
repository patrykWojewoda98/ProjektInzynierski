using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.RegionCode.GetAllRegionCodes
{
    public class GetAllRegionCodesQueryHandler : IRequestHandler<GetAllRegionCodesQuery, List<RegionCodeDto>>
    {
        private readonly IRegionCodeRepository _repository;

        public GetAllRegionCodesQueryHandler(IRegionCodeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RegionCodeDto>> Handle(GetAllRegionCodesQuery request, CancellationToken cancellationToken)
        {
            var regionCodes = await _repository.GetAllAsync(cancellationToken);

            return regionCodes.Select(r => new RegionCodeDto
            {
                Id = r.Id,
                Code = r.Code
            }).ToList();
        }
    }
}
