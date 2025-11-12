using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.WatchListItem.GetAllWatchListItems
{
    public class GetAllWatchListItemsQuery : IRequest<List<WatchListItemDto>>
    {
    }
}
