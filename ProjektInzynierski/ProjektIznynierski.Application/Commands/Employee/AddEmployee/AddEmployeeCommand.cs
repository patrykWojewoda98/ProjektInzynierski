using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Employee.AddEmployee
{
    public class AddEmployeeCommand : IRequest<EmployeeDto>
    {
        public string Name { get; set; }
        public bool IsAdmin { get; set; }

        public string Pesel { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}
