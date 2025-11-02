using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.WatchListItem.CreateWatchListItem
{
    internal class CreateWatchListItemCommandHandler : IRequestHandler<CreateWatchListItemCommand, WatchListItemDto>
    {
        private readonly IWatchListItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateWatchListItemCommandHandler(IWatchListItemRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WatchListItemDto> Handle(CreateWatchListItemCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.WatchListItem
            {
                WatchListId = request.WatchListId,
                InvestInstrumentId = request.InvestInstrumentId,
                AddedAt = request.AddedAt
            };
            _repository.Add(entity);
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
