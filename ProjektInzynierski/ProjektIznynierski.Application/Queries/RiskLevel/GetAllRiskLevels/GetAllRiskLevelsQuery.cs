using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.InvestProfile.GetAllInvestProfiles
{
    public class GetAllRiskLevelsQuery : IRequest<List<RiskLevelDto>>
    {
    }
}
