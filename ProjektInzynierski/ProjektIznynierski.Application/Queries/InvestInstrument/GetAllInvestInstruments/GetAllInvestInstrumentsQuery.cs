using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.InvestInstrument.GetAllByRegionId
{
    public record GetAllInvestInstrumentsQuery()
        : IRequest<List<InvestInstrumentDto>>;
}