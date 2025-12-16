using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.InvestInstrument.CreateInvestInstrument
{
    public class CreateInvestInstrumentCommand : IRequest<InvestInstrumentDto>
    {
        public string Name { get; set; }
        public string Ticker { get; set; }
        public int InvestmentTypeId { get; set; }
        public string Description { get; set; }
        public DateTime? MarketDataDate { get; set; }
        public int SectorId { get; set; }
        public int RegionId { get; set; }
        public int CountryId { get; set; }
        public int CurrencyId { get; set; }
        public int? FinancialMetricId { get; set; }
    }
}
