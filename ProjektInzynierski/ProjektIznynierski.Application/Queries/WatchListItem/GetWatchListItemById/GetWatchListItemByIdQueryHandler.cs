using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.WatchListItem.GetWatchListItemById
{
    internal class GetWatchListItemByIdQueryHandler : IRequestHandler<GetWatchListItemByIdQuery, WatchListItemDto>
    {
        private readonly IWatchListItemRepository _repository;
        public GetWatchListItemByIdQueryHandler(IWatchListItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<WatchListItemDto> Handle(GetWatchListItemByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"WatchListItem with id {request.id} not found.");
            }

            return new WatchListItemDto
            {
                Id = entity.Id,
                AddedAt = entity.AddedAt,
                WatchListId = entity.WatchListId,
                InvestInstrumentId = entity.InvestInstrumentId
            };
        }
    }
}
