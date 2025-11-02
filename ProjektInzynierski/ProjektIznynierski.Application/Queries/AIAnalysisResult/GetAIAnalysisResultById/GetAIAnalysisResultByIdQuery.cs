using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.AIAnalysisResult.GetAIAnalysisResultById
{
    public record GetAIAnalysisResultByIdQuery(int id) : IRequest<AIAnalysisResultDto>
    {
    }
}
