using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.InvestmentType.GetInvestmentTypeById
{
    public record GetInvestmentTypeByIdQuery(int id) : IRequest<InvestmentTypeDto>
    {
    }
}
