using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.AIAnalysisRequest.DeleteAIAnalysisRequest
{
    internal class DeleteAIAnalysisRequestCommandHandler : IRequestHandler<DeleteAIAnalysisRequestCommand, Unit>
    {
        private readonly IAIAnalisisRequestRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteAIAnalysisRequestCommandHandler(IAIAnalisisRequestRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteAIAnalysisRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"AIAnalysisRequest with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
