using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.MarketData.DeleteMarketData
{
    internal class DeleteMarketDataCommandHandler : IRequestHandler<DeleteMarketDataCommand, Unit>
    {
        private readonly IMarketDataRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteMarketDataCommandHandler(IMarketDataRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteMarketDataCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"MarketData with id {request.Id} not found.");
            }
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
