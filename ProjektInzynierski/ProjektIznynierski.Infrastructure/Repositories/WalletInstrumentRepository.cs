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
    }
}
