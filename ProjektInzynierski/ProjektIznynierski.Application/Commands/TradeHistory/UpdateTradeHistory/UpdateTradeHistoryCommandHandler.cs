using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.TradeHistory.UpdateTradeHistory
{
    internal class UpdateTradeHistoryCommandHandler : IRequestHandler<UpdateTradeHistoryCommand, TradeHistoryDto>
    {
        private readonly ITradeHistoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateTradeHistoryCommandHandler(ITradeHistoryRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TradeHistoryDto> Handle(UpdateTradeHistoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"TradeHistory with id {request.Id} not found.");
            }

            entity.WalletId = request.WalletId;
            entity.InvestInstrumentId = request.InvestInstrumentId;
            entity.Quantity = request.Quantity;
            entity.Price = request.Price;
            entity.TradeTypeId = request.TradeTypeId;
            entity.TradeDate = request.TradeDate;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

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
