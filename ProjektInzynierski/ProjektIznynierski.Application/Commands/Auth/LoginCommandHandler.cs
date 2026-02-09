using MediatR;
using ProjektIznynierski.Domain.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace ProjektIznynierski.Application.Commands.Auth.Login
{
    internal class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginCommandHandler(IClientRepository clientRepository, IJwtTokenService jwtTokenService)
        {
            _clientRepository = clientRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByEmailAsync(request.Email);

            if (client == null)
                throw new Exception("Invalid email or password.");

            var hash = ComputeSha256(request.Password);

            if (client.PasswordHash != hash)
                throw new Exception("Invalid email or password.");

            var token = await _jwtTokenService.GenerateToken(client);

            return new LoginResponseDto
            {
                Token = token,
                Message = "Login successful."
            };
        }

        private string ComputeSha256(string rawData)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToBase64String(hashBytes);
        }
    }
}
