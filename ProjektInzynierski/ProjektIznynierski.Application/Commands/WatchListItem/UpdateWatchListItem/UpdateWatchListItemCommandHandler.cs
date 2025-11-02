using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.WatchListItem.UpdateWatchListItem
{
    internal class UpdateWatchListItemCommandHandler : IRequestHandler<UpdateWatchListItemCommand, WatchListItemDto>
    {
        private readonly IWatchListItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateWatchListItemCommandHandler(IWatchListItemRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WatchListItemDto> Handle(UpdateWatchListItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"WatchListItem with id {request.Id} not found.");
            }

            entity.WatchListId = request.WatchListId;
            entity.InvestInstrumentId = request.InvestInstrumentId;
            entity.AddedAt = request.AddedAt;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return new WatchListItemDto
            {
                Id = entity.Id,
                WatchListId = entity.WatchListId,
                InvestInstrumentId = entity.InvestInstrumentId,
                AddedAt = entity.AddedAt
            };
        }
    }
}
