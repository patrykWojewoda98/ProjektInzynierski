namespace ProjektInzynierski.Domain.Models
{
    public class FinancialIndicatorsSnapshot
    {
        public decimal? DebtToEquity { get; set; }
        public decimal? DividendYield { get; set; }
        public decimal? PE { get; set; }
        public decimal? PB { get; set; }
        public decimal? ROE { get; set; }
    }
}
