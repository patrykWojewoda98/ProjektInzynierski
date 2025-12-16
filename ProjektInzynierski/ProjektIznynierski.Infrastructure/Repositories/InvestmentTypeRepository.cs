using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;

using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class InvestmentTypeRepository : GenericRepository<InvestmentType>, IInvestmentTypeRepository
    {
        public InvestmentTypeRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {

        }



    }
}
