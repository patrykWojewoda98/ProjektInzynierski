using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.InvestInstrument.GetInvestInstrumentById
{
    public record GetAllByTypeIdQuery(int id) : IRequest<List<InvestInstrumentDto>>;
};
