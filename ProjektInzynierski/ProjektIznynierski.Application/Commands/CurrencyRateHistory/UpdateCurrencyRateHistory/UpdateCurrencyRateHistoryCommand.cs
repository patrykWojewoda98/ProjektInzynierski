using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.CurrencyRateHistory.UpdateCurrencyRateHistory
{
    public class UpdateCurrencyRateHistoryCommand : IRequest<CurrencyRateHistoryDto>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal CloseRate { get; set; }
        public decimal? OpenRate { get; set; }
        public decimal? HighRate { get; set; }
        public decimal? LowRate { get; set; }
    }
}
