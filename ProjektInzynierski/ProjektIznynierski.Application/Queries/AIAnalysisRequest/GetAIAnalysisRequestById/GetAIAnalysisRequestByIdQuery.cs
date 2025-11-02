using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.AIAnalysisRequest.GetAIAnalysisRequestById
{
    public record GetAIAnalysisRequestByIdQuery(int id) : IRequest<AIAnalysisRequestDto>
    {
    }
}
