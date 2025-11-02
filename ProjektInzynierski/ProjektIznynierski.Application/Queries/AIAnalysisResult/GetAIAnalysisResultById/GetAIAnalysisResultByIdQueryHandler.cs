using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.AIAnalysisResult.GetAIAnalysisResultById
{
    internal class GetAIAnalysisResultByIdQueryHandler : IRequestHandler<GetAIAnalysisResultByIdQuery, AIAnalysisResultDto>
    {
        private readonly IAIAnalysisResultRepository _repository;
        public GetAIAnalysisResultByIdQueryHandler(IAIAnalysisResultRepository repository)
        {
            _repository = repository;
        }

        public async Task<AIAnalysisResultDto> Handle(GetAIAnalysisResultByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"AIAnalysisResult with id {request.id} not found.");
            }

            return new AIAnalysisResultDto
            {
                Id = entity.Id,
                Summary = entity.Summary,
                Recommendation = entity.Recommendation,
                KeyInsights = entity.KeyInsights,
                ConfidenceScore = entity.ConfidenceScore,
                ClientId = entity.ClientId
            };
        }
    }
}
