using MediatR;

namespace ProjektIznynierski.Application.Commands.FinancialReport.ImportFinancialReports
{
    public class ImportFinancialReportsCommand : IRequest<int>
    {
        public string Isin { get; set; }
    }
}
