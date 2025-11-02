using MediatR;
using ProjektIznynierski.Application.Comands.Client.AddClient;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;


namespace ProjektIznynierski.Application.Commands.Client.AddClient
{
    internal class AddClientCommandHandler : IRequestHandler<AddClientCommand, ClientDto>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ClientDto> Handle(AddClientCommand request, CancellationToken cancellationToken)
        {
            bool clientExists = await _clientRepository.CheckIfClientExist(request.Email);
            if (clientExists)
            {
                throw new Exception("Client with this email already exists.");
            }
            var client = new ProjektIznynierski.Domain.Entities.Client()
            {
                Name = request.Name,
                Email = request.Email,
                City = request.City,
                Address = request.Address,
                PostalCode = request.PostalCode,
                CountryId = request.CountryId,
                Wallet = request.Wallet,
                InvestProfile = request.InvestProfile,
                AIAnalysisResults = request.AIAnalysisResults,
                WatchLists = request.WatchLists
            };
            _clientRepository.Add(client);
            await _unitOfWork.SaveChangesAsync();

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

