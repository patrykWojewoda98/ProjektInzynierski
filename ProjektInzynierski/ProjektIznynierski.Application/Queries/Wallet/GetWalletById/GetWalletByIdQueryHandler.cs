using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Wallet.GetWalletById
{
    internal class GetWalletByIdQueryHandler : IRequestHandler<GetWalletByIdQuery, WalletDto>
    {
        private readonly IWalletRepository _walletRepository;
        public GetWalletByIdQueryHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<WalletDto> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(request.id, cancellationToken);
            if (wallet is null)
            {
                throw new Exception($"Wallet with id {request.id} not found.");
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
