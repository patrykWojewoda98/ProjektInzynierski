using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class WalletInstrumentRepository : GenericRepository<WalletInstrument>, IWalletInstrumentRepository
    {
        public WalletInstrumentRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<WalletInstrument>> GetByWalletIdAsync(int walletId, CancellationToken cancellationToken)
        {
            return await _dbContext.WalletInstruments
                .Where(x => x.WalletId == walletId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<WalletInstrument>> GetByWalletIdWithInstrumentAsync(int walletId,CancellationToken cancellationToken)
        {
            return await _dbContext.WalletInstruments
                .Include(x => x.InvestInstrument)
                .Where(x => x.WalletId == walletId)
                .ToListAsync(cancellationToken);
        }
    }
}
