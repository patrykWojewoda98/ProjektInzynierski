namespace ProjektIznynierski.Domain.Entities
{
    public class FinancialMetric : BaseEntity
    {
        public decimal? PE { get; set; }  // Price to Earnings
        public decimal? PB { get; set; }  // Price to Book
        public decimal? ROE { get; set; } // Return on Equity
        public decimal? DebtToEquity { get; set; }
        public decimal? DividendYield { get; set; }
    }
}
