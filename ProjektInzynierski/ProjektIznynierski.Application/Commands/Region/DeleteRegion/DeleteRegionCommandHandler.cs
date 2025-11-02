using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Region.DeleteRegion
{
    internal class DeleteRegionCommandHandler : IRequestHandler<DeleteRegionCommand, Unit>
    {
        private readonly IRegionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRegionCommandHandler(IRegionRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"Region with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
