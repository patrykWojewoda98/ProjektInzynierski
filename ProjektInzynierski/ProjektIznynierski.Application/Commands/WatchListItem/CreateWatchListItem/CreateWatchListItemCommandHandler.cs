using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;


namespace ProjektIznynierski.Application.Commands.WatchListItem.CreateWatchListItem
{
    internal class CreateWatchListItemCommandHandler
        : IRequestHandler<CreateWatchListItemCommand, WatchListItemDto>
    {
        private readonly IWatchListItemRepository _watchListItemRepository;
        private readonly IWatchListRepository _watchListRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWatchListItemCommandHandler(
            IWatchListItemRepository watchListItemRepository,
            IWatchListRepository watchListRepository,
            IUnitOfWork unitOfWork)
        {
            _watchListItemRepository = watchListItemRepository;
            _watchListRepository = watchListRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WatchListItemDto> Handle(CreateWatchListItemCommand request,CancellationToken cancellationToken)
        {
            var watchList =
                await _watchListRepository.FindOrCreateWatchListByClientIdAsync(
                    request.ClientId);

            var exists = await _watchListItemRepository.ExistsAsync(
                watchList.Id,
                request.InvestInstrumentId);

            if (exists)
            {
                return new WatchListItemDto
                {
                    WatchListId = watchList.Id,
                    InvestInstrumentId = request.InvestInstrumentId
                };
            }

            var entity = new Domain.Entities.WatchListItem
            {
                WatchList = watchList,
                InvestInstrumentId = request.InvestInstrumentId,
                AddedAt = DateTime.UtcNow
            };

            _watchListItemRepository.Add(entity);

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