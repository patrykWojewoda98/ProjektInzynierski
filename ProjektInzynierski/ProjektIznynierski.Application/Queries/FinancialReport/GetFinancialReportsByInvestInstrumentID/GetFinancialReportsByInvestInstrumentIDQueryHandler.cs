using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.FinancialReport.GetFinancialReportsByInvestInstrumentID
{
    internal class GetFinancialReportsByInvestInstrumentIDQueryHandler : IRequestHandler<GetFinancialReportsByInvestInstrumentIDQuery, List<FinancialReportDto>>
    {
        private readonly IFinancialReportRepository _repository;

        public GetFinancialReportsByInvestInstrumentIDQueryHandler(
            IFinancialReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FinancialReportDto>> Handle(
            GetFinancialReportsByInvestInstrumentIDQuery request,
            CancellationToken cancellationToken)
        {
            var reports = await _repository.GetByInstrumentIdAsync(
                request.id,
                cancellationToken
            );

            return reports.Select(r => new FinancialReportDto
            {
                Id = r.Id,
                Period = r.Period,
                Revenue = r.Revenue,
                NetIncome = r.NetIncome,
                EPS = r.EPS,
                Assets = r.Assets,
                Liabilities = r.Liabilities,
                OperatingCashFlow = r.OperatingCashFlow,
                FreeCashFlow = r.FreeCashFlow,
                InvestInstrumentId = r.InvestInstrumentId,
                ReportDate = r.ReportDate

            }).ToList();
        }
    }
}
