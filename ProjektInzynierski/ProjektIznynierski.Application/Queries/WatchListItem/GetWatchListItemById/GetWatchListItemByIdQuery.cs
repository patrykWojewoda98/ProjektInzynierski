using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.WatchListItem.GetWatchListItemById
{
    public record GetWatchListItemByIdQuery(int id) : IRequest<WatchListItemDto>
    {
    }
}
