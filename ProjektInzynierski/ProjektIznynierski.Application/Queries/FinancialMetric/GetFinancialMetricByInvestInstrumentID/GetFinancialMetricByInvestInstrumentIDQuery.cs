using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.FinancialReport.GetFinancialReportsByInvestInstrumentID
{
    public record GetFinancialMetricByInvestInstrumentIDQuery(int id) : IRequest<FinancialMetricDto>
    {
    }
}
