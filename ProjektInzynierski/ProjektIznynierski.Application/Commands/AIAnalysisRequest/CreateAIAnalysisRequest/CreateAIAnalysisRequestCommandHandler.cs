using MediatR;
using ProjektInzynierski.Application.Interfaces;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using System.Text;
using System.Text.Json;

namespace ProjektIznynierski.Application.Commands.AIAnalysisRequest.CreateAIAnalysisRequest
{
    internal class CreateAIAnalysisRequestCommandHandler : IRequestHandler<CreateAIAnalysisRequestCommand, AIAnalysisRequestDto>
    {
        private readonly IAIAnalisisRequestRepository _repository;
        private readonly IAIAnalysisResultRepository _analysisResultRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAIAnalysisPromptBuilder _promptBuilder;
        private readonly IChatGPTService _chatGPTService;
        public CreateAIAnalysisRequestCommandHandler(IAIAnalisisRequestRepository repository, IUnitOfWork unitOfWork, IAIAnalysisPromptBuilder promptBuilder, IChatGPTService chatGPTService, IAIAnalysisResultRepository analysisResultRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _promptBuilder = promptBuilder;
            _chatGPTService = chatGPTService;
            _analysisResultRepository = analysisResultRepository;
        }

        public async Task<AIAnalysisRequestDto> Handle(CreateAIAnalysisRequestCommand request,CancellationToken cancellationToken)
        {
            var prompt = await _promptBuilder.BuildPromptAsync(request.ClientId,request.InvestInstrumentId,cancellationToken);
            var aiResponse = await _chatGPTService.GetResponseAsync(prompt);

            var parsedResponse = JsonSerializer.Deserialize<AIAnalysisResultDto>( aiResponse,new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (parsedResponse == null)
                throw new Exception("AI returned invalid JSON");

           

            var analysisResult = new Domain.Entities.AIAnalysisResult
            {
                Summary = parsedResponse.Summary,
                Recommendation = parsedResponse.Recommendation,
                KeyInsights = parsedResponse.KeyInsights,
                ConfidenceScore = parsedResponse.ConfidenceScore,
                ClientId = request.ClientId
            };

            _analysisResultRepository.Add(analysisResult);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var entity = new Domain.Entities.AIAnalysisRequest
            {
                InvestInstrumentId = request.InvestInstrumentId,
                ClientId = request.ClientId,
                AIAnalysisResultId = analysisResult.Id
            };

            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AIAnalysisRequestDto
            {
                Id = entity.Id,
                InvestInstrumentId = entity.InvestInstrumentId,
                ClientId = entity.ClientId,
                AIAnalysisResultId = entity.AIAnalysisResultId,
                CreatedAt = entity.CreatedAt,

            };
        }

    }
}
