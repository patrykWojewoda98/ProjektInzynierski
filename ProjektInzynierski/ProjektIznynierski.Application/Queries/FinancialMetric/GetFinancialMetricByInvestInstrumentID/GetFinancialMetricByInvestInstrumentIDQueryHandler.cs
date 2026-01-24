using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.FinancialReport.GetFinancialReportsByInvestInstrumentID;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.FinancialMetric.GetFinancialMetricByInvestInstrumentID
{
    public class GetFinancialMetricByInvestInstrumentIDQueryHandler
        : IRequestHandler<GetFinancialMetricByInvestInstrumentIDQuery, FinancialMetricDto?>
    {
        private readonly IFinancialMetricRepository _repository;

        public GetFinancialMetricByInvestInstrumentIDQueryHandler(IFinancialMetricRepository repository)
        {
            _repository = repository;
        }

        public async Task<FinancialMetricDto?> Handle(GetFinancialMetricByInvestInstrumentIDQuery request,CancellationToken cancellationToken)
        {
            var metric = await _repository.GetByInvestInstrumentIdAsync(request.id, cancellationToken);

            if (metric == null)
                return null;

            return new FinancialMetricDto
            {
                Id = metric.Id,
                DebtToEquity = metric.DebtToEquity,
                DividendYield = metric.DividendYield,
                PE = metric.PE,
                PB = metric.PB,
                ROE = metric.ROE
            };
        }
    }
}
