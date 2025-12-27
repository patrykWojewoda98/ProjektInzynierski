using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.FinancialReport.UpdateFinancialReport
{
    internal class UpdateFinancialReportCommandHandler : IRequestHandler<UpdateFinancialReportCommand, FinancialReportDto>
    {
        private readonly IFinancialReportRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateFinancialReportCommandHandler(IFinancialReportRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<FinancialReportDto> Handle(UpdateFinancialReportCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"FinancialReport with id {request.Id} not found.");
            }

            entity.InvestInstrumentId = request.InvestInstrumentId;
            entity.Period = request.Period;
            entity.Revenue = request.Revenue;
            entity.NetIncome = request.NetIncome;
            entity.EPS = request.EPS;
            entity.Assets = request.Assets;
            entity.Liabilities = request.Liabilities;
            entity.OperatingCashFlow = request.OperatingCashFlow;
            entity.FreeCashFlow = request.FreeCashFlow;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return new FinancialReportDto
            {
                Id = entity.Id,
                InvestInstrumentId = entity.InvestInstrumentId,
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
