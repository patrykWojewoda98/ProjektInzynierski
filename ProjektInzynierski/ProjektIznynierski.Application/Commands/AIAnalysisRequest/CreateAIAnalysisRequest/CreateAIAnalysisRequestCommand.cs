using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.AIAnalysisRequest.CreateAIAnalysisRequest
{
    public class CreateAIAnalysisRequestCommand : IRequest<AIAnalysisRequestDto>
    {
        public int FinancialReportId { get; set; }
        public int InvestProfileId { get; set; }
    }
}
