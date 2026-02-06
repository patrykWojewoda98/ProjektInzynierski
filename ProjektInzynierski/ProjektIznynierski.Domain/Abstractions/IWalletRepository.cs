using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Task<List<WalletInstrument>> GetWalletInstrumentsByWalletIdAsync(int walletId);
        Task<Wallet> GetWalletByClientIdAsync(int clientId);
        Task<Wallet?> GetByIdWithCurrencyAsync(int walletId);
    }
}
