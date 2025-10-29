namespace ProjektIznynierski.Domain.Entities
{
    public class FinancialReport : BaseEntity
    {
        public int InvestInstrumentId { get; set; }
        public InvestInstrument InvestInstrument { get; set; }

        public DateTime ReportDate { get; set; }
        public string Period { get; set; } // e.g. "Q1 2024", "FY 2023"

        public decimal? Revenue { get; set; }
        public decimal? NetIncome { get; set; }
        public decimal? EPS { get; set; } // Earnings Per Share
        public decimal? Assets { get; set; }
        public decimal? Liabilities { get; set; }
        public decimal? OperatingCashFlow { get; set; }
        public decimal? FreeCashFlow { get; set; }
    }
}
