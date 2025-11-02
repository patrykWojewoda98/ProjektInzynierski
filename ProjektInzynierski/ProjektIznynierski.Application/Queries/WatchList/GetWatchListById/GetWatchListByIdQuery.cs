using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.WatchList.GetWatchListById
{
    public record GetWatchListByIdQuery(int id) : IRequest<WatchListDto>
    {
    }
}
