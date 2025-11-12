using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.Client.GetAllClients
{
    public class GetAllClientsQuery : IRequest<List<ClientDto>>
    {
    }
}
