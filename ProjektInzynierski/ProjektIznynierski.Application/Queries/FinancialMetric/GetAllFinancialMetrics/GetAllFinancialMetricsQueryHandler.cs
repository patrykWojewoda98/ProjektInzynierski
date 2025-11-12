using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.FinancialMetric.GetAllFinancialMetrics
{
    public class GetAllFinancialMetricsQueryHandler : IRequestHandler<GetAllFinancialMetricsQuery, List<FinancialMetricDto>>
    {
        private readonly IFinancialMetricRepository _financialMetricRepository;

        public GetAllFinancialMetricsQueryHandler(IFinancialMetricRepository financialMetricRepository)
        {
            _financialMetricRepository = financialMetricRepository;
        }

        public async Task<List<FinancialMetricDto>> Handle(GetAllFinancialMetricsQuery request, CancellationToken cancellationToken)
        {
            var financialMetrics = await _financialMetricRepository.GetAllAsync(cancellationToken);
            
            return financialMetrics.Select(fm => new FinancialMetricDto
            {
                Id = fm.Id,
                PE = fm.PE,
                PB = fm.PB,
                ROE = fm.ROE,
                DebtToEquity = fm.DebtToEquity,
                DividendYield = fm.DividendYield
            }).ToList();
        }
    }
}
