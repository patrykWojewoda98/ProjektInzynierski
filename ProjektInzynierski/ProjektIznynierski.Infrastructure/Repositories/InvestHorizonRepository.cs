using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class InvestHorizonRepository : GenericRepository<InvestHorizon>, IInvestHorizonRepository
    {
        public InvestHorizonRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {

        }
        

    }
}
