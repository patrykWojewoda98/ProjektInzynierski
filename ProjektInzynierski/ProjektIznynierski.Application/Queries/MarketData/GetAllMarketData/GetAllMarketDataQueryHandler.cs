using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.MarketData.GetAllMarketData
{
    public class GetAllMarketDataQueryHandler : IRequestHandler<GetAllMarketDataQuery, List<MarketDataDto>>
    {
        private readonly IMarketDataRepository _marketDataRepository;

        public GetAllMarketDataQueryHandler(IMarketDataRepository marketDataRepository)
        {
            _marketDataRepository = marketDataRepository;
        }

        public async Task<List<MarketDataDto>> Handle(GetAllMarketDataQuery request, CancellationToken cancellationToken)
        {
            var marketData = await _marketDataRepository.GetAllAsync(cancellationToken);
            
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
