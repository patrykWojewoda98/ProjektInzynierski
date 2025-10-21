using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class MarketDataRepository : GenericRepository<MarketData>, IMarketDataRepository
    {
        public MarketDataRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }
    }
}
