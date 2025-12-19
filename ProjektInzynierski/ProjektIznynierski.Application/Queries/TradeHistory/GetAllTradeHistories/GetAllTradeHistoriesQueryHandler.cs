using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.TradeHistory.GetAllTradeHistories
{
    public class GetAllTradeHistoriesQueryHandler : IRequestHandler<GetAllTradeHistoriesQuery, List<TradeHistoryDto>>
    {
        private readonly ITradeHistoryRepository _tradeHistoryRepository;

        public GetAllTradeHistoriesQueryHandler(ITradeHistoryRepository tradeHistoryRepository)
        {
            _tradeHistoryRepository = tradeHistoryRepository;
        }

        public async Task<List<TradeHistoryDto>> Handle(GetAllTradeHistoriesQuery request, CancellationToken cancellationToken)
        {
            var tradeHistories = await _tradeHistoryRepository.GetAllAsync(cancellationToken);
            
            return tradeHistories.Select(th => new TradeHistoryDto
            {
                Id = th.Id,
                WalletId = th.WalletId,
                InvestInstrumentId = th.InvestInstrumentId,
                Quantity = th.Quantity,
                Price = th.Price,
                TradeTypeId = th.TradeTypeId,
                TradeDate = th.TradeDate
            }).ToList();
        }
    }
}
