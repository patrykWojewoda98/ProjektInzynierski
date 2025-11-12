using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.AIAnalysisResult.GetAllAIAnalysisResults
{
    public class GetAllAIAnalysisResultsQuery : IRequest<List<AIAnalysisResultDto>>
    {
    }
}
