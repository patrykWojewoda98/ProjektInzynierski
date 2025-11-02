using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.AIAnalysisResult.UpdateAIAnalysisResult
{
    internal class UpdateAIAnalysisResultCommandHandler : IRequestHandler<UpdateAIAnalysisResultCommand, AIAnalysisResultDto>
    {
        private readonly IAIAnalysisResultRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateAIAnalysisResultCommandHandler(IAIAnalysisResultRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AIAnalysisResultDto> Handle(UpdateAIAnalysisResultCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"AIAnalysisResult with id {request.Id} not found.");
            }

            entity.Summary = request.Summary;
            entity.Recommendation = request.Recommendation;
            entity.KeyInsights = request.KeyInsights;
            entity.ConfidenceScore = request.ConfidenceScore;
            entity.ClientId = request.ClientId;

            _repository.Update(entity);
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
