using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.WatchListItem.DeleteWatchListItem
{
    internal class DeleteWatchListItemCommandHandler : IRequestHandler<DeleteWatchListItemCommand, Unit>
    {
        private readonly IWatchListItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteWatchListItemCommandHandler(IWatchListItemRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteWatchListItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"WatchListItem with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
