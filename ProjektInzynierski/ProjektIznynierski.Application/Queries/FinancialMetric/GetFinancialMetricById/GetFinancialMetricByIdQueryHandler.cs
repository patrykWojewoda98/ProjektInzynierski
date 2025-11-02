using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.FinancialMetric.GetFinancialMetricById
{
    internal class GetFinancialMetricByIdQueryHandler : IRequestHandler<GetFinancialMetricByIdQuery, FinancialMetricDto>
    {
        private readonly IFinancialMetricRepository _repository;
        public GetFinancialMetricByIdQueryHandler(IFinancialMetricRepository repository)
        {
            _repository = repository;
        }

        public async Task<FinancialMetricDto> Handle(GetFinancialMetricByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"FinancialMetric with id {request.id} not found.");
            }

            return new FinancialMetricDto
            {
                Id = entity.Id,
                PE = entity.PE,
                PB = entity.PB,
                ROE = entity.ROE,
                DebtToEquity = entity.DebtToEquity,
                DividendYield = entity.DividendYield
            };
        }
    }
}
