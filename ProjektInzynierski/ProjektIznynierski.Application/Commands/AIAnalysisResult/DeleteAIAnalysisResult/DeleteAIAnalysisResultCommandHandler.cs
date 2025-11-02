using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.AIAnalysisResult.DeleteAIAnalysisResult
{
    internal class DeleteAIAnalysisResultCommandHandler : IRequestHandler<DeleteAIAnalysisResultCommand, Unit>
    {
        private readonly IAIAnalysisResultRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteAIAnalysisResultCommandHandler(IAIAnalysisResultRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteAIAnalysisResultCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"AIAnalysisResult with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
