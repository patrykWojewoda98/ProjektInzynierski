using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Wallet.DeleteWallet
{
    internal class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletCommand, Unit>
    {
        private readonly IWalletRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteWalletCommandHandler(IWalletRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"Wallet with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
