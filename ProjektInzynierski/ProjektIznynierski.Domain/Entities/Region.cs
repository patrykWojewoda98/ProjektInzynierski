using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Domain.Entities
{
    public class Region : BaseEntity
    {
        public string Name { get; set; }

        public RegionCode Code { get; set; }

        public RiskLevel RegionRisk { get; set; }

        public ICollection<Country> Countries { get; set; }
    }
}
