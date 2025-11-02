using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Wallet.UpdateWallet
{
    internal class UpdateWalletCommandHandler : IRequestHandler<UpdateWalletCommand, WalletDto>
    {
        private readonly IWalletRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateWalletCommandHandler(IWalletRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletDto> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"Wallet with id {request.Id} not found.");
            }

            entity.ClientId = request.ClientId;
            entity.CashBalance = request.CashBalance;
            entity.CurrencyId = request.CurrencyId;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return new WalletDto
            {
                Id = entity.Id,
                ClientId = entity.ClientId,
                CashBalance = entity.CashBalance,
                CurrencyId = entity.CurrencyId
            };
        }
    }
}
