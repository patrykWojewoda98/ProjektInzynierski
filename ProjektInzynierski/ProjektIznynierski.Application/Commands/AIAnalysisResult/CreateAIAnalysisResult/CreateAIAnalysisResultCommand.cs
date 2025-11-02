using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.AIAnalysisResult.CreateAIAnalysisResult
{
    public class CreateAIAnalysisResultCommand : IRequest<AIAnalysisResultDto>
    {
        public string Summary { get; set; }
        public string Recommendation { get; set; }
        public string KeyInsights { get; set; }
        public decimal? ConfidenceScore { get; set; }
        public int ClientId { get; set; }
    }
}
