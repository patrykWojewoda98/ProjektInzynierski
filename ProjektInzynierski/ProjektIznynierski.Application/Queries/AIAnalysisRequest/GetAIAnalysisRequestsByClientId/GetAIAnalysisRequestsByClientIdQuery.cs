using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.AIAnalysisRequest.GetAIAnalysisRequestsByClientId
{
    public record GetAIAnalysisRequestsByClientIdQuery(int ClientId) : IRequest<List<AIAnalysisRequestDto>>
    {
    }
}
