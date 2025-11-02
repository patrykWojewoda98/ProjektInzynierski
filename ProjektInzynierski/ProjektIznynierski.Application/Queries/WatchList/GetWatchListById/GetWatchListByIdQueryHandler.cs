using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.WatchList.GetWatchListById
{
    internal class GetWatchListByIdQueryHandler : IRequestHandler<GetWatchListByIdQuery, WatchListDto>
    {
        private readonly IWatchListRepository _repository;
        public GetWatchListByIdQueryHandler(IWatchListRepository repository)
        {
            _repository = repository;
        }

        public async Task<WatchListDto> Handle(GetWatchListByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"WatchList with id {request.id} not found.");
            }

            return new WatchListDto
            {
                Id = entity.Id,
                ClientId = entity.ClientId,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
