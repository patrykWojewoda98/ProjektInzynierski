using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.AIAnalysisResult.UpdateAIAnalysisResult
{
    public class UpdateAIAnalysisResultCommand : IRequest<AIAnalysisResultDto>
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Recommendation { get; set; }
        public string KeyInsights { get; set; }
        public decimal? ConfidenceScore { get; set; }
        public int ClientId { get; set; }
    }
}
