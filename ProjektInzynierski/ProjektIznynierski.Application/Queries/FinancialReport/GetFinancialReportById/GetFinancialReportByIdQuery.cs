using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.FinancialReport.GetFinancialReportById
{
    public record GetFinancialReportByIdQuery(int id) : IRequest<FinancialReportDto>
    {
    }
}
