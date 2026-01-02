using FluentValidation;
using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace ProjektIznynierski.Application.Commands.Employee.UpdateEmployee
{
    internal class UpdateEmployeeCommandHandler
        : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateEmployeeCommand> _validator;

        public UpdateEmployeeCommandHandler(
            IEmployeeRepository repository,
            IUnitOfWork unitOfWork,
            IValidator<UpdateEmployeeCommand> validator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<EmployeeDto> Handle( UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            var employee = await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new Exception("Employee not found");

            employee.Name = request.Name;
            employee.IsAdmin = request.IsAdmin;
            employee.Email = request.Email;
            employee.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                using var sha = SHA256.Create();

                var oldPasswordHash = Convert.ToBase64String(
                    sha.ComputeHash(Encoding.UTF8.GetBytes(request.OldPassword)));

                if (employee.PasswordHash != oldPasswordHash)
                    throw new Exception("Old password is incorrect.");

                employee.PasswordHash = Convert.ToBase64String(
                    sha.ComputeHash(Encoding.UTF8.GetBytes(request.NewPassword)));
            }

            _repository.Update(employee);
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
