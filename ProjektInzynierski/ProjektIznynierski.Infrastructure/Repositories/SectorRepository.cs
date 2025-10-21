using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class SectorRepository : GenericRepository<Sector>, ISectorRepository
    {
        public SectorRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }
    }
}
