using MediatR;

namespace ProjektIznynierski.Application.Commands.AIAnalysisResult.DeleteAIAnalysisResult
{
    public class DeleteAIAnalysisResultCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
