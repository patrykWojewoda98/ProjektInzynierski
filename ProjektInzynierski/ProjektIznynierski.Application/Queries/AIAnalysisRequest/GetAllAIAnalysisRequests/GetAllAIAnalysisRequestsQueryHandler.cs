using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.AIAnalysisRequest.GetAllAIAnalysisRequests
{
    public class GetAllAIAnalysisRequestsQueryHandler : IRequestHandler<GetAllAIAnalysisRequestsQuery, List<AIAnalysisRequestDto>>
    {
        private readonly IAIAnalisisRequestRepository _repository;

        public GetAllAIAnalysisRequestsQueryHandler(IAIAnalisisRequestRepository aiAnalysisRequestRepository)
        {
            _repository = aiAnalysisRequestRepository;
        }

        public async Task<List<AIAnalysisRequestDto>> Handle(GetAllAIAnalysisRequestsQuery request, CancellationToken cancellationToken)
        {
            var aiAnalysisRequests = await _repository.GetAllAsync(cancellationToken);
            
            return aiAnalysisRequests.Select(a => new AIAnalysisRequestDto
            {
                Id = a.Id,
                FinancialReportId = a.FinancialReportId,
                InvestProfileId = a.InvestProfileId
            }).ToList();
        }
    }
}
