using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Commands.TradeHistory.CreateTradeHistory
{
    internal class CreateTradeHistoryCommandHandler : IRequestHandler<CreateTradeHistoryCommand, TradeHistoryDto>
    {
        private readonly ITradeHistoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateTradeHistoryCommandHandler(ITradeHistoryRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TradeHistoryDto> Handle(CreateTradeHistoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.TradeHistory
            {
                WalletId = request.WalletId,
                InvestInstrumentId = request.InvestInstrumentId,
                Quantity = request.Quantity,
                Price = request.Price,
                Type = (TradeType)request.Type,
                TradeDate = request.TradeDate
            };
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new TradeHistoryDto
            {
                Id = entity.Id,
                WalletId = entity.WalletId,
                InvestInstrumentId = entity.InvestInstrumentId,
                Quantity = entity.Quantity,
                Price = entity.Price,
                Type = (int)entity.Type,
                TradeDate = entity.TradeDate
            };
        }
    }
}
