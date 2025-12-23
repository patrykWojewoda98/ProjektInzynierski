using MediatR;
using ProjektIznynierski.Application.Commands.InvestmentType.DeleteInvestmentType;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Country.DeleteCountry
{
    internal class DeleteInvestmentTypeCommandHandler : IRequestHandler<DeleteInvestmentTypeCommand, Unit>
    {
        private readonly IInvestmentTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteInvestmentTypeCommandHandler(IInvestmentTypeRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteInvestmentTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"InvestmentType with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
