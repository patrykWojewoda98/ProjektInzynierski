using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.InvestHorizon.GetInvestHorizonByID
{
    public record GetInvestHorizonByIdQuery(int id) : IRequest<InvestHorizonDto>
    {
    }
}
