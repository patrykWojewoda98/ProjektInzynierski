using MediatR;

namespace ProjektIznynierski.Application.Commands.Wallet.DeleteWallet
{
    public class DeleteWalletCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
