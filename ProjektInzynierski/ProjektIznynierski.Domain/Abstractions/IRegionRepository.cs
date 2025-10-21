using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IRegionRepository : IRepository<Region>
    {
        Task<Region> GetRegionByCodeAsync(RegionCode code);

    }
}
