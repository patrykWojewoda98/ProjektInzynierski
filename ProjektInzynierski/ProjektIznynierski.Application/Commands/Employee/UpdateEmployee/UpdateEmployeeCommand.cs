using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Employee.UpdateEmployee
{
    public class UpdateEmployeeCommand : IRequest<EmployeeDto>
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public bool IsAdmin { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
