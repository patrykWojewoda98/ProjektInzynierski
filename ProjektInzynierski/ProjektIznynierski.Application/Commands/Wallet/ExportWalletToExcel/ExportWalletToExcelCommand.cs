using MediatR;

namespace ProjektIznynierski.Application.Commands.Wallet.ExportWalletToExcel
{
    public record ExportWalletToExcelCommand(int WalletId) : IRequest<byte[]>;
}
