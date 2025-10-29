using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Client.GetClients
{
    public record GetClientByIdQuery(int id) : IRequest<ClientDto>
    {
    }
}
