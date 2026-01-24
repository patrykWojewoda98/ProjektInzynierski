using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.FinancialMetric.DeleteFinancialMetric
{
    internal class DeleteFinancialMetricCommandHandler : IRequestHandler<DeleteFinancialMetricCommand, Unit>
    {
        private readonly IFinancialMetricRepository _repository;
        private readonly IInvestInstrumentRepository _instrumentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteFinancialMetricCommandHandler(IFinancialMetricRepository repository, IUnitOfWork unitOfWork, IInvestInstrumentRepository instrumentRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _instrumentRepository = instrumentRepository;
        }

        public async Task<Unit> Handle(DeleteFinancialMetricCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"FinancialMetric with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            var instrument = await _instrumentRepository.GetByFinacialMetricIdAsync(request.Id, cancellationToken);
            instrument.FinancialMetricId = null;

            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
