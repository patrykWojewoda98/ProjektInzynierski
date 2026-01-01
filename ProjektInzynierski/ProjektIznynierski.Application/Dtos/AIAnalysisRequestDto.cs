namespace ProjektIznynierski.Application.Dtos
{
    public class AIAnalysisRequestDto
    {
        public int Id { get; set; }

        public int InvestInstrumentId { get; set; }
        public int ClientId { get; set; }

        public int? AIAnalysisResultId { get; set; }

        public DateTime CreatedAt { get; set; }

        //TYMCZASOWO
        public string? Prompt { get; set; }
        public string? AIResponse { get; set; }
    }
}