using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.AIAnalysisRequest.CreateAIAnalysisRequest
{
    internal class CreateAIAnalysisRequestCommandHandler : IRequestHandler<CreateAIAnalysisRequestCommand, AIAnalysisRequestDto>
    {
        private readonly IAIAnalisisRequestRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateAIAnalysisRequestCommandHandler(IAIAnalisisRequestRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AIAnalysisRequestDto> Handle(CreateAIAnalysisRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.AIAnalysisRequest
            {
                FinancialReportId = request.FinancialReportId,
                InvestProfileId = request.InvestProfileId
            };
            _repository.Add(entity);
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
