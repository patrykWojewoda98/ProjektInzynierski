using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Domain.Entities
{
    public class Currency : BaseEntity
    {
        public string Name { get; set; }

        public int CurrencyRiskLevelId { get; set; }
    }
}
