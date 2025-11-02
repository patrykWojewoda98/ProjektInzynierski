using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.AIAnalysisRequest.UpdateAIAnalysisRequest
{
    internal class UpdateAIAnalysisRequestCommandHandler : IRequestHandler<UpdateAIAnalysisRequestCommand, AIAnalysisRequestDto>
    {
        private readonly IAIAnalisisRequestRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateAIAnalysisRequestCommandHandler(IAIAnalisisRequestRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AIAnalysisRequestDto> Handle(UpdateAIAnalysisRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"AIAnalysisRequest with id {request.Id} not found.");
            }

            entity.FinancialReportId = request.FinancialReportId;
            entity.InvestProfileId = request.InvestProfileId;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return new AIAnalysisRequestDto
            {
                Id = entity.Id,
                FinancialReportId = entity.FinancialReportId,
                InvestProfileId = entity.InvestProfileId
            };
        }
    }
}
