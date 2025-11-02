using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.InvestInstrument.GetInvestInstrumentById
{
    public record GetInvestInstrumentByIdQuery(int id) : IRequest<InvestInstrumentDto>
    {
    }
}
