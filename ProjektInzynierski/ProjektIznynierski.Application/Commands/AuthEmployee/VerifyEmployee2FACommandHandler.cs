using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace ProjektIznynierski.Application.Commands.AuthEmployee
{
    internal class VerifyEmployee2FACommandHandler: IRequestHandler<VerifyEmployee2FACommand, VerifyEmployee2FADto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public VerifyEmployee2FACommandHandler(IEmployeeRepository employeeRepository,IJwtTokenService jwtTokenService)
        {
            _employeeRepository = employeeRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<VerifyEmployee2FADto> Handle(VerifyEmployee2FACommand request,CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken) ?? throw new Exception("Employee not found");

            if (employee.TwoFactorCodeHash == null ||  employee.TwoFactorCodeExpiresAt == null)
            {
                return new VerifyEmployee2FADto
                {
                    Message = "2FA not initialized"
                };
            }

            if (employee.TwoFactorCodeExpiresAt < DateTime.UtcNow)
            {
                return new VerifyEmployee2FADto
                {
                    Message = "Verification code expired"
                };
            }

            var hashedInput = Hash(request.Code);

            if (employee.TwoFactorCodeHash != hashedInput)
            {
                return new VerifyEmployee2FADto
                {
                    Message = "Invalid verification code"
                };
            }


            var token = await _jwtTokenService.GenerateEployeeToken(employee);

            return new VerifyEmployee2FADto
            {
                Token = token,
                Message = "Login successful"
            };
        }

        private static string Hash(string value)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(value)));
        }
    }
}
