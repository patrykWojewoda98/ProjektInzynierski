using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Wallet.CreateWallet
{
    public class CreateWalletCommand : IRequest<WalletDto>
    {
        public int? ClientId { get; set; }
        public decimal CashBalance { get; set; }
        public int CurrencyId { get; set; }
    }
}
