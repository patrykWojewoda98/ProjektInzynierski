namespace ProjektIznynierski.Domain.Entities
{
    public class AIAnalysisRequest : BaseEntity
    {
        public int FinancialReportId { get; set; }
        public int InvestProfileId { get; set; }
    }
}
