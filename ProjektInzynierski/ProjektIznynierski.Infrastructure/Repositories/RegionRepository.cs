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

        public async Task<IEnumerable<Region>> GetAllRegionsAsync()
        {
            return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetRegionByCodeAsync(int regioncodeid)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Region>> GetRegionsByRiskLevelAsync(RiskLevel riskLevel)
        {
            throw new NotImplementedException();
        }
    }
}
