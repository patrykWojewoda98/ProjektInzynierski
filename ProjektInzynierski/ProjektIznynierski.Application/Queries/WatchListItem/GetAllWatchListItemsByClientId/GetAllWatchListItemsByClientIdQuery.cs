using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.WatchListItem.GetAllWatchListItemsByClientId
{
    public record GetAllWatchListItemsByClientIdQuery(int id) : IRequest<List<WatchListItemDto>>
    {
    }
}
