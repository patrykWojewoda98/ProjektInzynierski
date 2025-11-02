using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.FinancialReport.GetFinancialReportById
{
    internal class GetFinancialReportByIdQueryHandler : IRequestHandler<GetFinancialReportByIdQuery, FinancialReportDto>
    {
        private readonly IFinancialReportRepository _repository;
        public GetFinancialReportByIdQueryHandler(IFinancialReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<FinancialReportDto> Handle(GetFinancialReportByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"FinancialReport with id {request.id} not found.");
            }

            return new FinancialReportDto
            {
                Id = entity.Id,
                InvestInstrumentId = entity.InvestInstrumentId,
                ReportDate = entity.ReportDate,
                Period = entity.Period,
                Revenue = entity.Revenue,
                NetIncome = entity.NetIncome,
                EPS = entity.EPS,
                Assets = entity.Assets,
                Liabilities = entity.Liabilities,
                OperatingCashFlow = entity.OperatingCashFlow,
                FreeCashFlow = entity.FreeCashFlow
            };
        }
    }
}
