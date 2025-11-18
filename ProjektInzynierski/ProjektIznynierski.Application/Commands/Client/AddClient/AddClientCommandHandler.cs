using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace ProjektIznynierski.Application.Commands.Client.AddClient
{
    internal class AddClientCommandHandler : IRequestHandler<AddClientCommand, ClientDto>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddClientCommand> _validator;

        public AddClientCommandHandler(
            IClientRepository clientRepository,
            IUnitOfWork unitOfWork,
            IValidator<AddClientCommand> validator)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<ClientDto> Handle(AddClientCommand request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ValidationException($"Validation failed. Reasons: {string.Join(", ", errors)}");
            }

            bool clientExists = await _clientRepository.CheckIfClientExist(request.Email);
            if (clientExists)
                throw new Exception("Client with this email already exists.");

            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(request.Password);
            var hashBytes = sha256.ComputeHash(passwordBytes);
            var passwordHash = Convert.ToBase64String(hashBytes);

            var client = new Domain.Entities.Client
            {
                Name = request.Name,
                Email = request.Email,
                City = request.City,
                Address = request.Address,
                PostalCode = request.PostalCode,
                CountryId = request.CountryId,
                PasswordHash = passwordHash
            };

            _clientRepository.Add(client);
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
