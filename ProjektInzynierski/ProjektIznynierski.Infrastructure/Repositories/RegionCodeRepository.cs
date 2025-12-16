using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class RegionCodeRepository : GenericRepository<RegionCode>, IRegionCodeRepository
    {
        public RegionCodeRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {

        }


    }
}
