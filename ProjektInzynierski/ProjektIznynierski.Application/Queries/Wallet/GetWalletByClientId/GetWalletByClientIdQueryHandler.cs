using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;


namespace ProjektIznynierski.Application.Queries.Wallet.GetAllWallets
{
    public class GetWalletByClientIdQueryHandler
        : IRequestHandler<GetWalletByClientIdQuery, WalletDto>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetWalletByClientIdQueryHandler(IWalletRepository walletRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletDto> Handle(
            GetWalletByClientIdQuery request,
            CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository
                .GetWalletByClientIdAsync(request.ClientId);

            if (wallet == null)
            {
                wallet = new Domain.Entities.Wallet
                {
                    ClientId = request.ClientId,
                    CashBalance = 0,
                    CurrencyId = 1           
                };

                _walletRepository.Add(wallet);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return new WalletDto
            {
                Id = wallet.Id,
                ClientId = wallet.ClientId,
                CashBalance = wallet.CashBalance,
                CurrencyId = wallet.CurrencyId
            };
        }
    }
}
