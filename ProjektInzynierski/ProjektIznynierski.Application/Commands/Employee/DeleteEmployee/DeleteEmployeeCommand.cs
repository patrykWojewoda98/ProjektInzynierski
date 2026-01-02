using MediatR;

namespace ProjektIznynierski.Application.Commands.Employee.DeleteEmployee
{
    public class DeleteEmployeeCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}