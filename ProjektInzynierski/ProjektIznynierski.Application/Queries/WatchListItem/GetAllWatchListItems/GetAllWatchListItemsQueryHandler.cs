using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.WatchListItem.GetAllWatchListItems
{
    public class GetAllWatchListItemsQueryHandler : IRequestHandler<GetAllWatchListItemsQuery, List<WatchListItemDto>>
    {
        private readonly IWatchListItemRepository _watchListItemRepository;

        public GetAllWatchListItemsQueryHandler(IWatchListItemRepository watchListItemRepository)
        {
            _watchListItemRepository = watchListItemRepository;
        }

        public async Task<List<WatchListItemDto>> Handle(GetAllWatchListItemsQuery request, CancellationToken cancellationToken)
        {
            var watchListItems = await _watchListItemRepository.GetAllAsync(cancellationToken);
            
            return watchListItems.Select(wli => new WatchListItemDto
            {
                Id = wli.Id,
                AddedAt = wli.AddedAt,
                WatchListId = wli.WatchListId,
                InvestInstrumentId = wli.InvestInstrumentId
            }).ToList();
        }
    }
}
