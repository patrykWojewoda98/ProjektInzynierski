using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.RiskLevel.DeleteRiskLevel
{
    internal class DeleteRiskLevelCommandHandler
        : IRequestHandler<DeleteRiskLevelCommand, Unit>
    {
        private readonly IRiskLevelRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRiskLevelCommandHandler(
            IRiskLevelRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(
            DeleteRiskLevelCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"RiskLevel with id {request.Id} not found.");
            }

            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
