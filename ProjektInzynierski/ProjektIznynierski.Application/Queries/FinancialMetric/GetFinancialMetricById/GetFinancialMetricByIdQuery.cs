using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.FinancialMetric.GetFinancialMetricById
{
    public record GetFinancialMetricByIdQuery(int id) : IRequest<FinancialMetricDto>
    {
    }
}
