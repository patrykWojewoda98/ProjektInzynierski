using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.WalletInstrument.DeleteWalletInstrument
{
    internal class DeleteWalletInstrumentCommandHandler : IRequestHandler<DeleteWalletInstrumentCommand, Unit>
    {
        private readonly IWalletInstrumentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteWalletInstrumentCommandHandler(IWalletInstrumentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteWalletInstrumentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"WalletInstrument with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
