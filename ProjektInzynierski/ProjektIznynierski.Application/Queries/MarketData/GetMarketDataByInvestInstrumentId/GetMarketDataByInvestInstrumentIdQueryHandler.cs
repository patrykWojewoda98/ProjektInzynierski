using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.MarketData.GetMarketDataByInvestInstrumentId;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.InvestInstrument.GetAllInvestInstruments
{
    public class GetMarketDataByInvestInstrumentIdQueryHandler : IRequestHandler<GetMarketDataByInvestInstrumentIdQuery, List<MarketDataDto>>
    {
        private readonly IMarketDataRepository _marketDataRepository;

        public GetMarketDataByInvestInstrumentIdQueryHandler(IMarketDataRepository marketDataRepository)
        {
            _marketDataRepository = marketDataRepository;
        }

        public async Task<List<MarketDataDto>> Handle(GetMarketDataByInvestInstrumentIdQuery request, CancellationToken cancellationToken)
        {
            var marketData = await _marketDataRepository.GetByInvestInstrumentIdAsync(request.id, cancellationToken);
            return marketData.Select(md => new MarketDataDto
            {
                Id = md.Id,
                InvestInstrumentId = md.InvestInstrumentId,
                Date = md.Date,
                OpenPrice = md.OpenPrice,
                ClosePrice = md.ClosePrice,
                HighPrice = md.HighPrice,
                LowPrice = md.LowPrice,
                Volume = md.Volume,
                DailyChange = md.DailyChange,
                DailyChangePercent = md.DailyChangePercent
            }).ToList();
        }
    }
}
