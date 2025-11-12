using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Wallet.GetAllWallets
{
    public class GetAllWalletsQueryHandler : IRequestHandler<GetAllWalletsQuery, List<WalletDto>>
    {
        private readonly IWalletRepository _walletRepository;

        public GetAllWalletsQueryHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<List<WalletDto>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
        {
            var wallets = await _walletRepository.GetAllAsync(cancellationToken);
            
            return wallets.Select(w => new WalletDto
            {
                Id = w.Id,
                ClientId = w.ClientId,
                CashBalance = w.CashBalance,
                CurrencyId = w.CurrencyId
            }).ToList();
        }
    }
}
