using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.InvestHorizon.GetAllInvestHorizons
{
    public class GetAllInvestHorizonsQuery : IRequest<List<InvestHorizonDto>>
    {
    }
}
