using MediatR;

namespace ProjektIznynierski.Application.Commands.FinancialMetric.ImportFinancialIndicators
{
    public class ImportFinancialMetricCommand : IRequest<int>
    {
        public int InvestInstrumentId { get; init; }
    }
}
