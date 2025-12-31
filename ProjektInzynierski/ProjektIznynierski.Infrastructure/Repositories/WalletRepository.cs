using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Wallet?> GetWalletByClientIdAsync(int clientId)
        {
            return await _dbContext.Wallets
                .FirstOrDefaultAsync(w => w.ClientId == clientId);
        }


        public async Task<List<WalletInstrument>> GetWalletInstrumentsByWalletIdAsync(int walletId)
        {
            return await _dbContext.WalletInstruments
                .Where(wi => wi.WalletId == walletId)
                .ToListAsync();
        }
    }
}
