using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.InvestProfile.DeleteInvestProfile
{
    internal class DeleteInvestProfileCommandHandler : IRequestHandler<DeleteInvestProfileCommand, Unit>
    {
        private readonly IInvestProfileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteInvestProfileCommandHandler(IInvestProfileRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteInvestProfileCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"InvestProfile with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
