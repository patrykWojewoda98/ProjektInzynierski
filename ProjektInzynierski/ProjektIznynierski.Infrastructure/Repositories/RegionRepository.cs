using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Domain.Enums;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class RegionRepository : IRegionRepository
    {
        private readonly ProjektInzynierskiDbContext _dbContext;
        public RegionRepository(ProjektInzynierskiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Region> GetRegiionByIdAsync(int id)
        {
            return await _dbContext.Regions.SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Region> GetRegionByCodeAsync(RegionCode code)
        {
            return await _dbContext.Regions.SingleOrDefaultAsync(r => r.Code == code);
        }

        public void AddRegion(Region region)
        {
            _dbContext.Regions.Add(region);
        }

        public void UpdateRegion(Region region)
        {
            _dbContext.Regions.Update(region);
        }

        public void RemoveRegion(Region region)
        {
            _dbContext.Regions.Remove(region);
        }
    }
}
