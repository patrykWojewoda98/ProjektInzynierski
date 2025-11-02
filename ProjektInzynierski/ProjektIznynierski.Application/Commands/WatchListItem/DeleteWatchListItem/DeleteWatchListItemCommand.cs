using MediatR;

namespace ProjektIznynierski.Application.Commands.WatchListItem.DeleteWatchListItem
{
    public class DeleteWatchListItemCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
