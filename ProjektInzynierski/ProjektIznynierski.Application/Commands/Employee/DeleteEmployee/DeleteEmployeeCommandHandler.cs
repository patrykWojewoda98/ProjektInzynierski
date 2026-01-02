using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Employee.DeleteEmployee
{
    internal class DeleteEmployeeCommandHandler
        : IRequestHandler<DeleteEmployeeCommand, Unit>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeCommandHandler(IEmployeeRepository repository,IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteEmployeeCommand request,CancellationToken cancellationToken)
        {
            var employee = await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new Exception("Employee not found");

            _repository.Delete(employee);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
