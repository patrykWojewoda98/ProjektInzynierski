using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.FinancialMetric.DeleteFinancialMetric
{
    internal class DeleteFinancialMetricCommandHandler : IRequestHandler<DeleteFinancialMetricCommand, Unit>
    {
        private readonly IFinancialMetricRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteFinancialMetricCommandHandler(IFinancialMetricRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteFinancialMetricCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"FinancialMetric with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
