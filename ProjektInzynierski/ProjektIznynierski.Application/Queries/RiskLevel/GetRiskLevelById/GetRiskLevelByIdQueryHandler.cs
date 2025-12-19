using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.InvestProfile.GetInvestProfileById
{
    internal class GetRiskLevelByIdQueryHandler : IRequestHandler<GetRiskLevelByIdQuery, RiskLevelDto>
    {
        private readonly IRiskLevelRepository _repository;
        public GetRiskLevelByIdQueryHandler(IRiskLevelRepository repository)
        {
            _repository = repository;
        }

        public async Task<RiskLevelDto> Handle(GetRiskLevelByIdQuery request, CancellationToken cancellationToken)
        {
            var riskLevel = await _repository.GetByIdAsync(request.id, cancellationToken);
            if (riskLevel is null)
            {
                throw new Exception($"RiskLevel with id {request.id} not found.");
            }

            return new RiskLevelDto
            {
                Id = riskLevel.Id,
                RiskScale = riskLevel.RiskLevelScale,
                Description = riskLevel.Description
            };
        }
    }
}
