using ProjektIznynierski.Domain.Entities;


namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IRiskLevelRepository : IRepository<RiskLevel>
    {
        Task<bool> ExistsByRiskScaleAsync(int riskScale);
        Task<RiskLevel> GetMaxRiskLevel();
    }
}
