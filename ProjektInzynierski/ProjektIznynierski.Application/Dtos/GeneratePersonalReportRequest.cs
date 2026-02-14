namespace ProjektIznynierski.Application.Dtos
{
    public class GeneratePersonalReportRequest
    {
        public int ClientId { get; set; }
        public int? InvestInstrumentId { get; set; }
        public bool IncludeInstrumentInfo { get; set; }
        public bool IncludeFinancialMetrics { get; set; }
        public List<string>? IncludedMetricFields { get; set; }
        public bool IncludeFinancialReports { get; set; }
        public List<int>? IncludedFinancialReportIds { get; set; }
        public bool IncludePortfolioComposition { get; set; }
        public string? CustomIntroText { get; set; }
        public string? CustomOutroText { get; set; }
        public string? FontFamily { get; set; }
        public int? FontSize { get; set; }
    }
}
