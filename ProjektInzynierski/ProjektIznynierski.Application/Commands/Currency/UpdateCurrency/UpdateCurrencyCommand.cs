using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Currency.UpdateCurrency
{
    public class UpdateCurrencyCommand : IRequest<CurrencyDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrencyRiskLevelId { get; set; }
    }
}
