using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.WatchList.DeleteWatchList
{
    internal class DeleteWatchListCommandHandler : IRequestHandler<DeleteWatchListCommand, Unit>
    {
        private readonly IWatchListRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteWatchListCommandHandler(IWatchListRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteWatchListCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"WatchList with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
