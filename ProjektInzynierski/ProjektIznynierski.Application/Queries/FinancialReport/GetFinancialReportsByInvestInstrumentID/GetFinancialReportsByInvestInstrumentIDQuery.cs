using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.FinancialReport.GetFinancialReportsByInvestInstrumentID
{
    public record GetFinancialReportsByInvestInstrumentIDQuery(int id) : IRequest<List<FinancialReportDto>>
    {
    }
}
