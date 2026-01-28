using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.CurrencyPair.DeleteCurrencyPair
{
    internal class DeleteCurrencyPairCommandHandler : IRequestHandler<DeleteCurrencyPairCommand, Unit>
    {
        private readonly ICurrencyPairRepository _currencyPairRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCurrencyPairCommandHandler(ICurrencyPairRepository currencyPairRepository,IUnitOfWork unitOfWork)
        {
            _currencyPairRepository = currencyPairRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteCurrencyPairCommand request, CancellationToken cancellationToken)
        {
            var entity = await _currencyPairRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
                throw new Exception($"Currency pair with id {request.Id} not found.");

            _currencyPairRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
