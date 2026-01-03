using MediatR;
using ProjektIznynierski.Domain.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace ProjektIznynierski.Application.Commands.AuthAdmin.Login
{
    internal class LoginEmployeeCommandHandler: IRequestHandler<LoginEmployeeCommand, LoginResponseDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITwoFactorCodeService _twoFactorService;

        public LoginEmployeeCommandHandler(IEmployeeRepository employeeRepository,IEmailService emailService,IUnitOfWork unitOfWork, ITwoFactorCodeService twoFactorService)
        {
            _employeeRepository = employeeRepository;
            _twoFactorService = twoFactorService;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResponseDto> Handle(LoginEmployeeCommand request,CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByEmailAsync(request.Email);
            if (employee == null)
                throw new Exception("Invalid email or password.");

            var hash = ComputeSha256(request.Password);
            if (employee.PasswordHash != hash)
                throw new Exception("Invalid email or password.");

            var code = _twoFactorService.GenerateCode();
            employee.TwoFactorCodeHash = _twoFactorService.HashCode(code);
            employee.TwoFactorCodeExpiresAt = DateTime.UtcNow.AddMinutes(5);

            _employeeRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _emailService.SendAsync(
                employee.Email,
                "Your verification code",
                $"""
                <h3>Employee login verification</h3>
                <p>Your 6-digit code:</p>
                <h2>{code}</h2>
                <p>This code expires in 5 minutes.</p>
                """
            );

            return new LoginResponseDto
            {
                EmployeeId = employee.Id,
                Message = "Verification code sent to email."
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
