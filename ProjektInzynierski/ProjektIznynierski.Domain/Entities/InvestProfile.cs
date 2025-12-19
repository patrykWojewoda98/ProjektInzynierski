

namespace ProjektIznynierski.Domain.Entities
{
    public class InvestProfile : BaseEntity
    {

        public string ProfileName { get; set; } // np. "Aggressive", "Crypto-focused"

        public int AcceptableRiskLevelId { get; set; }

        public int? InvestHorizonId { get; set; }
        public InvestHorizon? InvestHorizon { get; set; }

        public double? TargetReturn { get; set; }
        public double? MaxDrawDown { get; set; }

        public ICollection<Region> PreferredRegions { get; set; }
        public ICollection<Sector> PreferredSectors { get; set; }

        public int? ClientId { get; set; }
        public Client? Client { get; set; }
    }
}
