using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Employee.GetEmployeeById
{
    public record GetEmployeeByIdQuery(int id) : IRequest<EmployeeDto>;
}
