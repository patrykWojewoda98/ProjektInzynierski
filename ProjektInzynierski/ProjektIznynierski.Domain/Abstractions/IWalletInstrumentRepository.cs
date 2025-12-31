using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IWalletInstrumentRepository : IRepository<WalletInstrument>
    {
        Task<List<WalletInstrument>> GetByWalletIdAsync(int walletId, CancellationToken cancellationToken = default);
        Task<List<WalletInstrument>> GetByWalletIdWithInstrumentAsync(int walletId,CancellationToken cancellationToken);
    }
}
