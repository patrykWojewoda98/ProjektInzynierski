namespace ProjektIznynierski.Application.Dtos
{
    public class InvestInstrumentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public DateTime? MarketDataDate { get; set; }
        public int SectorId { get; set; }
        public int RegionId { get; set; }
        public int CountryId { get; set; }
        public int CurrencyId { get; set; }
        public int? FinancialMetricId { get; set; }
    }
}
