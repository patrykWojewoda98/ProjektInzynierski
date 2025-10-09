namespace ProjektIznynierski.Domain.Entities
{
    public class AIAnalysisResult : BaseEntity
    {
        public string Summary { get; set; }
        public string Recommendation { get; set; } // Buy / Hold / Sell
        public string KeyInsights { get; set; }    // main points from the analysis
        public decimal ConfidenceScore { get; set; } // e.g. 0.87 (87%)
    }
}
