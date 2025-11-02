using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Client.UpdateClient
{
    internal class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, ClientDto>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClientDto> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByIdAsync(request.Id, cancellationToken);
            if (client is null)
            {
                throw new Exception($"Client with id {request.Id} not found.");
            }

            client.Name = request.Name;
            client.Email = request.Email;
            client.City = request.City;
            client.Address = request.Address;
            client.PostalCode = request.PostalCode;
            client.CountryId = request.CountryId;

            _clientRepository.Update(client);
            await _unitOfWork.SaveChangesAsync();

            return new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                City = client.City,
                Address = client.Address,
                PostalCode = client.PostalCode
            };
        }
    }
}
