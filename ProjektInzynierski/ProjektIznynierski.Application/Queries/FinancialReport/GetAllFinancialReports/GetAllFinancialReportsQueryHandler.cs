using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.FinancialReport.GetAllFinancialReports
{
    public class GetAllFinancialReportsQueryHandler : IRequestHandler<GetAllFinancialReportsQuery, List<FinancialReportDto>>
    {
        private readonly IFinancialReportRepository _financialReportRepository;

        public GetAllFinancialReportsQueryHandler(IFinancialReportRepository financialReportRepository)
        {
            _financialReportRepository = financialReportRepository;
        }

        public async Task<List<FinancialReportDto>> Handle(GetAllFinancialReportsQuery request, CancellationToken cancellationToken)
        {
            var financialReports = await _financialReportRepository.GetAllAsync(cancellationToken);
            
            return financialReports.Select(fr => new FinancialReportDto
            {
                Id = fr.Id,
                InvestInstrumentId = fr.InvestInstrumentId,
                Period = fr.Period,
                Revenue = fr.Revenue,
                NetIncome = fr.NetIncome,
                EPS = fr.EPS,
                Assets = fr.Assets,
                Liabilities = fr.Liabilities,
                OperatingCashFlow = fr.OperatingCashFlow,
                FreeCashFlow = fr.FreeCashFlow
            }).ToList();
        }
    }
}
