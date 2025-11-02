using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.AIAnalysisRequest.GetAIAnalysisRequestById
{
    internal class GetAIAnalysisRequestByIdQueryHandler : IRequestHandler<GetAIAnalysisRequestByIdQuery, AIAnalysisRequestDto>
    {
        private readonly IAIAnalisisRequestRepository _repository;
        public GetAIAnalysisRequestByIdQueryHandler(IAIAnalisisRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<AIAnalysisRequestDto> Handle(GetAIAnalysisRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"AIAnalysisRequest with id {request.id} not found.");
            }

            return new AIAnalysisRequestDto
            {
                Id = entity.Id,
                FinancialReportId = entity.FinancialReportId,
                InvestProfileId = entity.InvestProfileId
            };
        }
    }
}
