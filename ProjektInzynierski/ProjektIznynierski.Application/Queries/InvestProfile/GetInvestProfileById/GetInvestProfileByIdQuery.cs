using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.InvestProfile.GetInvestProfileById
{
    public record GetInvestProfileByIdQuery(int id) : IRequest<InvestProfileDto>
    {
    }
}
