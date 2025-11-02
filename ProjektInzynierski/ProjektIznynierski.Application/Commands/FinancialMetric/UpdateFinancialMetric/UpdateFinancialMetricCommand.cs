using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.FinancialMetric.UpdateFinancialMetric
{
    public class UpdateFinancialMetricCommand : IRequest<FinancialMetricDto>
    {
        public int Id { get; set; }
        public decimal? PE { get; set; }
        public decimal? PB { get; set; }
        public decimal? ROE { get; set; }
        public decimal? DebtToEquity { get; set; }
        public decimal? DividendYield { get; set; }
    }
}
