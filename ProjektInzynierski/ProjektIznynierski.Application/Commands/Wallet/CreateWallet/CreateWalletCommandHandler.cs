using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Wallet.CreateWallet
{
    internal class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, WalletDto>
    {
        private readonly IWalletRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateWalletCommandHandler(IWalletRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletDto> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Wallet
            {
                ClientId = request.ClientId,
                CashBalance = request.CashBalance,
                CurrencyId = request.CurrencyId
            };
            _repository.Add(entity);
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
