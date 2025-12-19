using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.TradeHistory.GetTradeHistoryById
{
    internal class GetTradeHistoryByIdQueryHandler : IRequestHandler<GetTradeHistoryByIdQuery, TradeHistoryDto>
    {
        private readonly ITradeHistoryRepository _repository;
        public GetTradeHistoryByIdQueryHandler(ITradeHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<TradeHistoryDto> Handle(GetTradeHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"TradeHistory with id {request.id} not found.");
            }

            return new TradeHistoryDto
            {
                Id = entity.Id,
                WalletId = entity.WalletId,
                InvestInstrumentId = entity.InvestInstrumentId,
                Quantity = entity.Quantity,
                Price = entity.Price,
                TradeTypeId = entity.TradeTypeId,
                TradeDate = entity.TradeDate
            };
        }
    }
}
