using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace ProjektIznynierski.Application.Commands.Employee.AddEmployee
{
    internal class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, EmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddEmployeeCommand> _validator;

        public AddEmployeeCommandHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork, IValidator<AddEmployeeCommand> validator)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<EmployeeDto> Handle(AddEmployeeCommand request,CancellationToken cancellationToken)
        {
            ValidationResult validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                throw new ValidationException(string.Join(", ", errors));
            }

            if (await _employeeRepository.CheckIfEmployeeExistsAsync(request.Email))
                throw new Exception("Employee with this email already exists.");

            using var sha256 = SHA256.Create();
            var passwordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));

            var employee = new Domain.Entities.Employee
            {
                Name = request.Name,
                IsAdmin = request.IsAdmin,
                Pesel = request.Pesel,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                PasswordHash = passwordHash
            };

            _employeeRepository.Add(employee);
            await _unitOfWork.SaveChangesAsync();

            return new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Pesel = employee.Pesel,
                IsAdmin = employee.IsAdmin
            };
        }
    }
}
