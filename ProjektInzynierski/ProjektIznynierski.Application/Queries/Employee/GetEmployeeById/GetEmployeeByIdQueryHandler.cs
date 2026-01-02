using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Employee.GetEmployeeById
{
    internal class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request,CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.id, cancellationToken);

            if (employee is null)
            {
                throw new Exception($"Employee with id {request.id} not found.");
            }

            return new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                IsAdmin = employee.IsAdmin,
                Pesel = employee.Pesel
            };
        }
    }
}
