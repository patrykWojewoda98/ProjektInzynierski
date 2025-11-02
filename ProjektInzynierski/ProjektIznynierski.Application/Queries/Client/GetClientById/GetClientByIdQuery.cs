using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Client.GetClientById
{
    public record GetClientByIdQuery(int id) : IRequest<ClientDto>
    {
    }
}
