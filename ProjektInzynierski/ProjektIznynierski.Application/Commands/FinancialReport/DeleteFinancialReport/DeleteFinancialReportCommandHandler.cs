using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.FinancialReport.DeleteFinancialReport
{
    internal class DeleteFinancialReportCommandHandler : IRequestHandler<DeleteFinancialReportCommand, Unit>
    {
        private readonly IFinancialReportRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteFinancialReportCommandHandler(IFinancialReportRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteFinancialReportCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"FinancialReport with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
