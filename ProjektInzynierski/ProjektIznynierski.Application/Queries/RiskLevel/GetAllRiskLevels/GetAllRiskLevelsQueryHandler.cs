using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.InvestProfile.GetAllInvestProfiles
{
    public class GetAllRiskLevelsQueryHandler : IRequestHandler<GetAllRiskLevelsQuery, List<RiskLevelDto>>
    {
        private readonly IRiskLevelRepository _repository;

        public GetAllRiskLevelsQueryHandler(IRiskLevelRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RiskLevelDto>> Handle(GetAllRiskLevelsQuery request, CancellationToken cancellationToken)
        {
            var riskLevels = await _repository.GetAllAsync(cancellationToken);

            return riskLevels.Select(r => new RiskLevelDto
            {
                Id = r.Id,
                RiskScale = r.RiskLevelScale,
                Description = r.Description

            }).ToList();
        }
    }
}
