using FluentValidation;
using MediatR;
using ProjektIznynierski.Domain.Abstractions;
namespace ProjektIznynierski.Application.Commands.InvestHorizon.DeleteInvestHorizon
{
    internal class DeleteInvestHorizonCommandHandler : IRequestHandler<DeleteInvestHorizonCommand, Unit>
    {
        private readonly IInvestHorizonRepository _investHorizonRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInvestHorizonCommandHandler(
            IInvestHorizonRepository investHorizonRepository,
            IUnitOfWork unitOfWork)
        {
            _investHorizonRepository = investHorizonRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteInvestHorizonCommand request, CancellationToken cancellationToken)
        {
            var entity = await _investHorizonRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"InvestHorizon  with id {request.Id} not found.");
            }
            _investHorizonRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}