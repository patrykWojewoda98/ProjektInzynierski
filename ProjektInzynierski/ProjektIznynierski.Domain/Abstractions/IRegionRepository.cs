using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IRegionRepository
    {
        Task<Region> GetRegiionByIdAsync(int id);
        Task<Region> GetRegionByCodeAsync(RegionCode code);
        void AddRegion(Region region);
        void UpdateRegion(Region region);
        void RemoveRegion(Region region);

    }
}
