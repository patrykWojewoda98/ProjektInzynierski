using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.CurrencyPair.UpdateCurrencyPair
{
    public class UpdateCurrencyPairCommand : IRequest<CurrencyPairDto>
    {
        public int Id { get; set; }
        public int BaseCurrencyId { get; set; }
        public int QuoteCurrencyId { get; set; }
        public string Symbol { get; set; } = string.Empty;
    }
}
