using System;

namespace ProjektIznynierski.Application.Dtos
{
    public class AIAnalysisRequestDto
    {
        public int Id { get; set; }
        public string RequestData { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public int? ClientId { get; set; }
        public int FinancialReportId { get; set; }
        public int InvestProfileId { get; set; }
    }
}
