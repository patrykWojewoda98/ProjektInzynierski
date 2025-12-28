using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.WatchListItem.CreateWatchListItem
{
    public class CreateWatchListItemCommand : IRequest<WatchListItemDto>
    {
        public int ClientId { get; set; }
        public int InvestInstrumentId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
