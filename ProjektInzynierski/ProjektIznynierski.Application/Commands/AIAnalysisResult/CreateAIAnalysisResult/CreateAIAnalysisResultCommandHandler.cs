using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.AIAnalysisResult.CreateAIAnalysisResult
{
    internal class CreateAIAnalysisResultCommandHandler : IRequestHandler<CreateAIAnalysisResultCommand, AIAnalysisResultDto>
    {
        private readonly IAIAnalysisResultRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateAIAnalysisResultCommandHandler(IAIAnalysisResultRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AIAnalysisResultDto> Handle(CreateAIAnalysisResultCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.AIAnalysisResult
            {
                Summary = request.Summary,
                Recommendation = request.Recommendation,
                KeyInsights = request.KeyInsights,
                ConfidenceScore = request.ConfidenceScore,
                ClientId = request.ClientId
            };
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

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
