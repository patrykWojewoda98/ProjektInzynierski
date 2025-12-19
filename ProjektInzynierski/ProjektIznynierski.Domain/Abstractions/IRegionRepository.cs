using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IRegionRepository : IRepository<Region>
    {
        Task<Region> GetRegionByCodeAsync(int code);
        Task<IEnumerable<Region>> GetAllRegionsAsync();
        Task<IEnumerable<Region>> GetRegionsByRiskLevelAsync(RiskLevel riskLevel);

    }
}
