using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.AIAnalysisRequest.CreateAIAnalysisRequest
{
    public class CreateAIAnalysisRequestCommand: IRequest<AIAnalysisRequestDto>
    {
        public int InvestInstrumentId { get; set; }
        public int ClientId { get; set; }
    }
}
