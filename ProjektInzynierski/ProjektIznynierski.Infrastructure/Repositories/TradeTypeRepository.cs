using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class TradeTypeRepository : GenericRepository<TradeType>, ITradeTypeRepository
    {
        public TradeTypeRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {

        }


    }
}
