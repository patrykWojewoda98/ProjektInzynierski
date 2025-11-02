using FluentValidation;
using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using FluentValidation.Results;

namespace ProjektIznynierski.Application.Commands.Client.UpdateClient
{
    internal class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, ClientDto>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateClientCommand> _validator;
        public UpdateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork, IValidator<UpdateClientCommand> validator)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<ClientDto> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ValidationException($"Walidacja nie powiod³a sie. Przyczyny: {string.Join(", ", errors)}");
            }


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
