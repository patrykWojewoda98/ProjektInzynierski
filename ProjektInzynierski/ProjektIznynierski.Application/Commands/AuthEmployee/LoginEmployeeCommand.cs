using MediatR;

namespace ProjektIznynierski.Application.Commands.AuthAdmin.Login
{
    public class LoginEmployeeCommand : IRequest<LoginResponseDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}