using MediatR;
using ProjektInzynierski.Application.Interfaces;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
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
        private readonly IEmailService _emailService;
        private readonly IClientRepository _clientRepository;
        private readonly IInvestInstrumentRepository _investInstrumentRepository;
        public CreateAIAnalysisRequestCommandHandler(IAIAnalisisRequestRepository repository, IUnitOfWork unitOfWork,IInvestInstrumentRepository investInstrumentRepository, IAIAnalysisPromptBuilder promptBuilder,IClientRepository clientRepository, IChatGPTService chatGPTService, IAIAnalysisResultRepository analysisResultRepository, IEmailService emailService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _promptBuilder = promptBuilder;
            _chatGPTService = chatGPTService;
            _analysisResultRepository = analysisResultRepository;
            _emailService = emailService;
            _clientRepository = clientRepository;
            _investInstrumentRepository = investInstrumentRepository;
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

            var client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
            var clientName = client.Name;
            var investInstrument = await _investInstrumentRepository.GetByIdAsync(request.InvestInstrumentId, cancellationToken);
            var instrumentName = investInstrument.Name;

            await _emailService.SendAsync(
                  client.Email,
                  "AI Analysis Results",
                  $"""
                  <p>Hi {client.Name},</p>

                  <p>
                      here are the AI analysis results for <strong>{investInstrument.Name}</strong>.
                  </p>

                  <hr />

                  <p><strong>Summary:</strong></p>
                  <p>{parsedResponse.Summary}</p>

                  <p><strong>Recommendation:</strong></p>
                  <p>{parsedResponse.Recommendation}</p>

                  <p><strong>Key insights:</strong></p>
                  <p>{parsedResponse.KeyInsights}</p>

                  <p><strong>Confidence score:</strong></p>
                  <h2>{parsedResponse.ConfidenceScore}</h2>
                  """ );

            //do testów
            await _emailService.SendAsync(
                  client.Email, "Test Promptu", prompt);

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
