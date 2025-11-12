using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.FinancialMetric.GetAllFinancialMetrics
{
    public class GetAllFinancialMetricsQuery : IRequest<List<FinancialMetricDto>>
    {
    }
}
