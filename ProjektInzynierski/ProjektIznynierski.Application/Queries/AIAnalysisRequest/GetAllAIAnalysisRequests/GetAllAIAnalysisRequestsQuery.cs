using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.AIAnalysisRequest.GetAllAIAnalysisRequests
{
    public class GetAllAIAnalysisRequestsQuery : IRequest<List<AIAnalysisRequestDto>>
    {
    }
}
