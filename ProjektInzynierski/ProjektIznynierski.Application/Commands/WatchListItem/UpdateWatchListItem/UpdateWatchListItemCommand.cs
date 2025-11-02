using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.WatchListItem.UpdateWatchListItem
{
    public class UpdateWatchListItemCommand : IRequest<WatchListItemDto>
    {
        public int Id { get; set; }
        public int WatchListId { get; set; }
        public int InvestInstrumentId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
