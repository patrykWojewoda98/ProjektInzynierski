using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.FinancialReport.CreateFinancialReport
{
    internal class CreateFinancialReportCommandHandler : IRequestHandler<CreateFinancialReportCommand, FinancialReportDto>
    {
        private readonly IFinancialReportRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateFinancialReportCommandHandler(IFinancialReportRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<FinancialReportDto> Handle(CreateFinancialReportCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.FinancialReport
            {
                InvestInstrumentId = request.InvestInstrumentId,
                ReportDate = request.ReportDate,
                Period = request.Period,
                Revenue = request.Revenue,
                NetIncome = request.NetIncome,
                EPS = request.EPS,
                Assets = request.Assets,
                Liabilities = request.Liabilities,
                OperatingCashFlow = request.OperatingCashFlow,
                FreeCashFlow = request.FreeCashFlow
            };
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

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
