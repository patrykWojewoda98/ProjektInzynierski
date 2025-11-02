using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Sector.DeleteSector
{
    internal class DeleteSectorCommandHandler : IRequestHandler<DeleteSectorCommand, Unit>
    {
        private readonly ISectorRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteSectorCommandHandler(ISectorRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteSectorCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"Sector with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
