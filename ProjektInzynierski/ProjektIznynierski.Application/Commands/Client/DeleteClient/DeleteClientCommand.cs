using MediatR;

namespace ProjektIznynierski.Application.Commands.Client.DeleteClient
{
    public class DeleteClientCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
