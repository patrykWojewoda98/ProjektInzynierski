using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Domain.Entities
{
    public class Currency : BaseEntity
    {
        public string Name { get; set; }

        public RiskLevel CurrencyRisk { get; set; }
    }
}
