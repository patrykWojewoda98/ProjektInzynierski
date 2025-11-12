using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.FinancialReport.GetAllFinancialReports
{
    public class GetAllFinancialReportsQuery : IRequest<List<FinancialReportDto>>
    {
    }
}
