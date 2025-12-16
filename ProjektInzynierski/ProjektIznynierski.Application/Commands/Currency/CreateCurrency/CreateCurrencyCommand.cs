using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Currency.CreateCurrency
{
    public class CreateCurrencyCommand : IRequest<CurrencyDto>
    {
        public string Name { get; set; }
        public int CurrencyRiskLevelId { get; set; }
    }
}
