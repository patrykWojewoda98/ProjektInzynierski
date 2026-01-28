using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.CurrencyRateHistory.AddCurrencyRateHistory
{
    public class AddCurrencyRateHistoryCommand : IRequest<CurrencyRateHistoryDto>
    {
        public int CurrencyPairId { get; set; }
        public DateTime Date { get; set; }
        public decimal CloseRate { get; set; }
        public decimal? OpenRate { get; set; }
        public decimal? HighRate { get; set; }
        public decimal? LowRate { get; set; }
    }
}
