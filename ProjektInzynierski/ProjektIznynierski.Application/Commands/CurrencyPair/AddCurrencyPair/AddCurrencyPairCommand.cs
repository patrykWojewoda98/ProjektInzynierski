using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.CurrencyPair.AddCurrencyPair
{
    public class AddCurrencyPairCommand : IRequest<CurrencyPairDto>
    {
        public int BaseCurrencyId { get; set; }
        public int QuoteCurrencyId { get; set; }
        public string Symbol { get; set; } = string.Empty;
    }
}
