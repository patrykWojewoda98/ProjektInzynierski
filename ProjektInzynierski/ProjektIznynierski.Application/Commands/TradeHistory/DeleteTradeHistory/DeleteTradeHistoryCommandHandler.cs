using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.TradeHistory.DeleteTradeHistory
{
    internal class DeleteTradeHistoryCommandHandler : IRequestHandler<DeleteTradeHistoryCommand, Unit>
    {
        private readonly ITradeHistoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteTradeHistoryCommandHandler(ITradeHistoryRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteTradeHistoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"TradeHistory with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
