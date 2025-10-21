using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Domain.Enums;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class RegionRepository : GenericRepository<Region>, IRegionRepository
    {
        public RegionRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Region> GetRegionByCodeAsync(RegionCode code)
        {
            return await _dbContext.Regions.SingleOrDefaultAsync(r => r.Code == code);
        }
    }
}
