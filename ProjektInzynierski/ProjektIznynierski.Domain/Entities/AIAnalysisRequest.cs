namespace ProjektIznynierski.Domain.Entities
{
    public class AIAnalysisRequest : BaseEntity
    {
        public int InvestInstrumentId { get; set; }
        public int ClientId { get; set; }

        public int? AIAnalysisResultId { get; set; }

        public AIAnalysisResult? AIAnalysisResult { get; set; }
    }
}