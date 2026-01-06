using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.FinancialMetric.CreateFinancialMetric
{
    internal class CreateFinancialMetricAndAssignToInstrumentCommandHandler : IRequestHandler<CreateFinancialMetricAndAssignToInstrumentCommand, FinancialMetricDto>
    {
        private readonly IFinancialMetricRepository _repository;
        private readonly IInvestInstrumentRepository _investInstrumentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateFinancialMetricAndAssignToInstrumentCommandHandler(IFinancialMetricRepository repository, IUnitOfWork unitOfWork, IInvestInstrumentRepository investInstrumentRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _investInstrumentRepository = investInstrumentRepository;
        }

        public async Task<FinancialMetricDto> Handle(CreateFinancialMetricAndAssignToInstrumentCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.FinancialMetric
            {
                PE = request.PE,
                PB = request.PB,
                ROE = request.ROE,
                DebtToEquity = request.DebtToEquity,
                DividendYield = request.DividendYield
            };
            var instrument = await _investInstrumentRepository
                .GetByIdAsync(request.InvestInstrumentId, cancellationToken);

            if (instrument == null)
                throw new Exception("Investment instrument not found.");

            if (instrument.FinancialMetricId != null)
                throw new Exception(
                    "Financial metric already exists for this investment instrument."
                );

            _repository.Add(entity);
            instrument.FinancialMetric = entity;
            await _unitOfWork.SaveChangesAsync();
            instrument.FinancialMetricId = entity.Id;
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new FinancialMetricDto
            {
                Id = entity.Id,
                PE = entity.PE,
                PB = entity.PB,
                ROE = entity.ROE,
                DebtToEquity = entity.DebtToEquity,
                DividendYield = entity.DividendYield
            };
        }
    }
}
