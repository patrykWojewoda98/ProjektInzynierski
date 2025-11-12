using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.WatchList.GetAllWatchLists
{
    public class GetAllWatchListsQuery : IRequest<List<WatchListDto>>
    {
    }
}
