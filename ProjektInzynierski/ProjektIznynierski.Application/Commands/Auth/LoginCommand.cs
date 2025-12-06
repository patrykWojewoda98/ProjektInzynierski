using MediatR;

namespace ProjektIznynierski.Application.Commands.Auth.Login
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}