using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.InvestInstrument.DeleteInvestInstrument
{
    internal class DeleteInvestInstrumentCommandHandler : IRequestHandler<DeleteInvestInstrumentCommand, Unit>
    {
        private readonly IInvestInstrumentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteInvestInstrumentCommandHandler(IInvestInstrumentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteInvestInstrumentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"InvestInstrument with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
