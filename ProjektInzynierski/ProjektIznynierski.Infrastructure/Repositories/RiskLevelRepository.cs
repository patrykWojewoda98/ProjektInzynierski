using Microsoft.EntityFrameworkCore;
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
        public async Task<bool> ExistsByRiskScaleAsync(int riskScale)
        {
            return await _dbContext.RiskLevels
                .AnyAsync(x => x.RiskLevelScale == riskScale);
        }

        public async Task<RiskLevel> GetMaxRiskLevel()
        {
            return await _dbContext.RiskLevels
                .OrderByDescending(x => x.RiskLevelScale).FirstAsync();
        }

    }
}
