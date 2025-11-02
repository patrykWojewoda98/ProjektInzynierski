using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.WatchList.CreateWatchList
{
    public class CreateWatchListCommand : IRequest<WatchListDto>
    {
        public int ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
