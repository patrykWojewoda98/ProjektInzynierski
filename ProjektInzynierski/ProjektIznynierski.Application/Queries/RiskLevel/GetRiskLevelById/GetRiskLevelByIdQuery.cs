using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.InvestProfile.GetInvestProfileById
{
    public record GetRiskLevelByIdQuery(int id) : IRequest<RiskLevelDto>
    {
    }
}
