using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.CurrencyRateHistory.DeleteCurrencyRateHistory
{
    internal class DeleteCurrencyRateHistoryCommandHandler: IRequestHandler<DeleteCurrencyRateHistoryCommand, Unit>
    {
        private readonly ICurrencyRateHistoryRepository _rateHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCurrencyRateHistoryCommandHandler(ICurrencyRateHistoryRepository rateHistoryRepository,IUnitOfWork unitOfWork)
        {
            _rateHistoryRepository = rateHistoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteCurrencyRateHistoryCommand request,CancellationToken cancellationToken)
        {
            var entity = await _rateHistoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
                throw new Exception( $"Currency rate history with id {request.Id} not found.");

            _rateHistoryRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
