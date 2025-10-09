using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Domain.Entities
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }

        public string IsoCode { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public RiskLevel CountryRisk { get; set; }
    }
}
