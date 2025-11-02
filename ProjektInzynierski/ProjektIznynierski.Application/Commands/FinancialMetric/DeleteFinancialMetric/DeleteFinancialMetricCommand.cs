using MediatR;

namespace ProjektIznynierski.Application.Commands.FinancialMetric.DeleteFinancialMetric
{
    public class DeleteFinancialMetricCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
