using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class RiskLevelRepository : GenericRepository<RiskLevel>, IRiskLevelRepository
    {
        public RiskLevelRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {

        }


    }
}
