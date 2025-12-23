using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.InvestInstrument.GetAllByRegionId
{
    public record GetAllInvestmentTypesQuery()
        : IRequest<List<InvestmentTypeDto>>;
}