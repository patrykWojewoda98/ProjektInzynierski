using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Wallet.UpdateWallet
{
    public class UpdateWalletCommand : IRequest<WalletDto>
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public decimal CashBalance { get; set; }
        public int CurrencyId { get; set; }
    }
}
