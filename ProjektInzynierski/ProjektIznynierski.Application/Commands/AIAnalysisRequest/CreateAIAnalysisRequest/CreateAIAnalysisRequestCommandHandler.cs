using MediatR;
using ProjektInzynierski.Application.Interfaces;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using System.Text;

namespace ProjektIznynierski.Application.Commands.AIAnalysisRequest.CreateAIAnalysisRequest
{
    internal class CreateAIAnalysisRequestCommandHandler : IRequestHandler<CreateAIAnalysisRequestCommand, AIAnalysisRequestDto>
    {
        private readonly IAIAnalisisRequestRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAIAnalysisPromptBuilder _promptBuilder;
        private readonly IChatGPTService _chatGPTService;
        public CreateAIAnalysisRequestCommandHandler(IAIAnalisisRequestRepository repository, IUnitOfWork unitOfWork, IAIAnalysisPromptBuilder promptBuilder, IChatGPTService chatGPTService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _promptBuilder = promptBuilder;
            _chatGPTService = chatGPTService;
        }

        public async Task<AIAnalysisRequestDto> Handle(CreateAIAnalysisRequestCommand request,CancellationToken cancellationToken)
        {
            var prompt = await _promptBuilder.BuildPromptAsync(request.ClientId,request.InvestInstrumentId,cancellationToken);
            var aiResponse = await _chatGPTService.GetResponseAsync(prompt);

            var entity = new Domain.Entities.AIAnalysisRequest
            {
                InvestInstrumentId = request.InvestInstrumentId,
                ClientId = request.ClientId,

            };

            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new AIAnalysisRequestDto
            {
                Id = entity.Id,
                InvestInstrumentId = entity.InvestInstrumentId,
                ClientId = entity.ClientId,
                AIAnalysisResultId = entity.AIAnalysisResultId,
                CreatedAt = entity.CreatedAt,
                AIResponse = aiResponse,
                Prompt = prompt

            };
        }

    }
}
