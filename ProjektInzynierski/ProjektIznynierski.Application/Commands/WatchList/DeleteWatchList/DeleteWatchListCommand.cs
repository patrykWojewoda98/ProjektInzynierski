using MediatR;

namespace ProjektIznynierski.Application.Commands.WatchList.DeleteWatchList
{
    public class DeleteWatchListCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
