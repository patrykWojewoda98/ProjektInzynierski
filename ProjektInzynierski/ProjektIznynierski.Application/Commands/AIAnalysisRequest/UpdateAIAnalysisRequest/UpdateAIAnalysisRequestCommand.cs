using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.AIAnalysisRequest.UpdateAIAnalysisRequest
{
    public class UpdateAIAnalysisRequestCommand : IRequest<AIAnalysisRequestDto>
    {
        public int Id { get; set; }
        public int FinancialReportId { get; set; }
        public int InvestProfileId { get; set; }
    }
}
