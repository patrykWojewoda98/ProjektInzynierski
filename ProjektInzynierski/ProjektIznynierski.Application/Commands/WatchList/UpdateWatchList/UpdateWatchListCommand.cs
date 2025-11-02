using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.WatchList.UpdateWatchList
{
    public class UpdateWatchListCommand : IRequest<WatchListDto>
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
