using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.WatchList.GetAllWatchLists
{
    public class GetAllWatchListsQueryHandler : IRequestHandler<GetAllWatchListsQuery, List<WatchListDto>>
    {
        private readonly IWatchListRepository _watchListRepository;

        public GetAllWatchListsQueryHandler(IWatchListRepository watchListRepository)
        {
            _watchListRepository = watchListRepository;
        }

        public async Task<List<WatchListDto>> Handle(GetAllWatchListsQuery request, CancellationToken cancellationToken)
        {
            var watchLists = await _watchListRepository.GetAllAsync(cancellationToken);
            
            return watchLists.Select(wl => new WatchListDto
            {
                Id = wl.Id,
                ClientId = wl.ClientId,
                CreatedAt = wl.CreatedAt
            }).ToList();
        }
    }
}
