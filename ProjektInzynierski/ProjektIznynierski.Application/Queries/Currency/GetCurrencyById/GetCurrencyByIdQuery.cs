using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Currency.GetCurrencyById
{
    public record GetCurrencyByIdQuery(int id) : IRequest<CurrencyDto>
    {
    }
}
