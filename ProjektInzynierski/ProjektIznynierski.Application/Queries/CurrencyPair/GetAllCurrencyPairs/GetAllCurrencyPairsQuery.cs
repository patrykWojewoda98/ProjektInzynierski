using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.CurrencyPair.GetAllCurrencyPairs
{
    public record GetAllCurrencyPairsQuery() : IRequest<List<CurrencyPairDto>>
    {
    }
}
