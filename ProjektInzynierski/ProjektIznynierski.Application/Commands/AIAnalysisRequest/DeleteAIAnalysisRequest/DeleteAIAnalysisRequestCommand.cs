using MediatR;

namespace ProjektIznynierski.Application.Commands.AIAnalysisRequest.DeleteAIAnalysisRequest
{
    public class DeleteAIAnalysisRequestCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
