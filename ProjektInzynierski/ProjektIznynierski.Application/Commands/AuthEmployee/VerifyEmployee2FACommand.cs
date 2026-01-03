using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.AuthEmployee
{
    public class VerifyEmployee2FACommand : IRequest<VerifyEmployee2FADto>
    {
        public int EmployeeId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
