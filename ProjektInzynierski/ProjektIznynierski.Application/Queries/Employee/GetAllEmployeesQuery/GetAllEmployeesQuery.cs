using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Employee.GetAllEmployees
{
    public class GetAllEmployeesQuery : IRequest<List<EmployeeDto>>
    {
    }
}
