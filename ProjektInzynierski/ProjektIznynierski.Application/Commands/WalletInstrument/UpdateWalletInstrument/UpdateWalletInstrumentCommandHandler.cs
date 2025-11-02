using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.WalletInstrument.UpdateWalletInstrument
{
    internal class UpdateWalletInstrumentCommandHandler : IRequestHandler<UpdateWalletInstrumentCommand, WalletInstrumentDto>
    {
        private readonly IWalletInstrumentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateWalletInstrumentCommandHandler(IWalletInstrumentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletInstrumentDto> Handle(UpdateWalletInstrumentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"WalletInstrument with id {request.Id} not found.");
            }

            entity.WalletId = request.WalletId;
            entity.InvestInstrumentId = request.InvestInstrumentId;
            entity.Quantity = request.Quantity;

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return new WalletInstrumentDto
            {
                Id = entity.Id,
                WalletId = entity.WalletId,
                InvestInstrumentId = entity.InvestInstrumentId,
                Quantity = entity.Quantity
            };
        }
    }
}
