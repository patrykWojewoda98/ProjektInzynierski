using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.AIAnalysisRequest.GetAIAnalysisRequestsByClientId
{
    public class GetAIAnalysisRequestsByClientIdQueryHandler : IRequestHandler<GetAIAnalysisRequestsByClientIdQuery, List<AIAnalysisRequestDto>>
    {
        private readonly IAIAnalisisRequestRepository _repository;

        public GetAIAnalysisRequestsByClientIdQueryHandler(IAIAnalisisRequestRepository aiAnalysisRequestRepository)
        {
            _repository = aiAnalysisRequestRepository;
        }

        public async Task<List<AIAnalysisRequestDto>> Handle(GetAIAnalysisRequestsByClientIdQuery request, CancellationToken cancellationToken)
        {
            var aiAnalysisRequests = await _repository.GetByClientIdAsync(request.ClientId, cancellationToken);

            return aiAnalysisRequests.Select(x => new AIAnalysisRequestDto
                {
                    Id = x.Id,
                    ClientId = x.ClientId,
                    InvestInstrumentId = x.InvestInstrumentId,
                    AIAnalysisResultId = x.AIAnalysisResultId,
                    CreatedAt = x.CreatedAt
                })
                .ToList();
        }
    }
}
