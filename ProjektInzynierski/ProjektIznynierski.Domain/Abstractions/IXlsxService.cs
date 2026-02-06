using ProjektInzynierski.Domain.Models;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IXlsxService
    {
        byte[] GenerateWalletExcel(WalletSnapshot walletSnapshot);
    }
}
