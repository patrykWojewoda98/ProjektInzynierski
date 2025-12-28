using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.WatchListItem.GetAllWatchListItemsByClientId
{
    public class GetAllWatchListItemsByClientIdQueryHandler : IRequestHandler<GetAllWatchListItemsByClientIdQuery, List<WatchListItemDto>>
    {
        private readonly IWatchListItemRepository _watchListItemRepository;

        public GetAllWatchListItemsByClientIdQueryHandler(IWatchListItemRepository watchListItemRepository)
        {
            _watchListItemRepository = watchListItemRepository;
        }
        public async Task<List<WatchListItemDto>> Handle(GetAllWatchListItemsByClientIdQuery request, CancellationToken cancellationToken)
        {
            var watchListItems = await _watchListItemRepository.GetAllWatchListItemsByClientId(request.id, cancellationToken);

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
