

namespace ProjektIznynierski.Domain.Entities
{
    public class InvestInstrument : BaseEntity
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public int InvestmentTypeId { get; set; }
        public string Isin { get; set; }

        public string Description { get; set; }
        public DateTime? MarketDataDate { get; set; }

        // Foreign keys
        public int SectorId { get; set; }
        public Sector Sector { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public int? FinancialMetricId { get; set; }
        public FinancialMetric FinancialMetric { get; set; }

        public ICollection<MarketData> MarketData { get; set; } = new List<MarketData>();
        public ICollection<FinancialReport> FinancialReports { get; set; }
    }
}