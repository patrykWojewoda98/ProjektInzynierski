using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.FinancialReport.CreateFinancialReport
{
    public class CreateFinancialReportCommand : IRequest<FinancialReportDto>
    {
        public int InvestInstrumentId { get; set; }
        public string Period { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? NetIncome { get; set; }
        public decimal? EPS { get; set; }
        public decimal? Assets { get; set; }
        public decimal? Liabilities { get; set; }
        public decimal? OperatingCashFlow { get; set; }
        public decimal? FreeCashFlow { get; set; }
    }
}
