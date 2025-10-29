using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Client.GetClients
{
    internal class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDto>
    {
        private readonly IClientRepository _clientRepository;
        public GetClientByIdQueryHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ClientDto> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByIdAsync(request.id);

            var clientDto = new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                City = client.City,
                Address = client.Address,
                PostalCode = client.PostalCode
            };
            return clientDto;
        }
    }
}
