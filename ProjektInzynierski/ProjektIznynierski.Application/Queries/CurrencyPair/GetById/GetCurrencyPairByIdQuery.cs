using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.CurrencyPair.GetById
{
    public record GetCurrencyPairByIdQuery(int Id): IRequest<CurrencyPairDto>
    {
    }
}
