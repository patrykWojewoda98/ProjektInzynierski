using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.AIAnalysisResult.GetAllAIAnalysisResults
{
    public class GetAllAIAnalysisResultsQueryHandler : IRequestHandler<GetAllAIAnalysisResultsQuery, List<AIAnalysisResultDto>>
    {
        private readonly IAIAnalysisResultRepository _aiAnalysisResultRepository;

        public GetAllAIAnalysisResultsQueryHandler(IAIAnalysisResultRepository aiAnalysisResultRepository)
        {
            _aiAnalysisResultRepository = aiAnalysisResultRepository;
        }

        public async Task<List<AIAnalysisResultDto>> Handle(GetAllAIAnalysisResultsQuery request, CancellationToken cancellationToken)
        {
            var aiAnalysisResults = await _aiAnalysisResultRepository.GetAllAsync(cancellationToken);
            
            return aiAnalysisResults.Select(a => new AIAnalysisResultDto
            {
                Id = a.Id,
                Summary = a.Summary,
                Recommendation = a.Recommendation,
                KeyInsights = a.KeyInsights,
                ConfidenceScore = a.ConfidenceScore,
                ClientId = a.ClientId
            }).ToList();
        }
    }
}
