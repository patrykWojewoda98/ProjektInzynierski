using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Task<List<WalletInstrument>> GetWalletInstrumentsByWalletIdAsync(int walletId);
    }
}
