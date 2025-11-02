using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.WalletInstrument.CreateWalletInstrument
{
    internal class CreateWalletInstrumentCommandHandler : IRequestHandler<CreateWalletInstrumentCommand, WalletInstrumentDto>
    {
        private readonly IWalletInstrumentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateWalletInstrumentCommandHandler(IWalletInstrumentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletInstrumentDto> Handle(CreateWalletInstrumentCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.WalletInstrument
            {
                WalletId = request.WalletId,
                InvestInstrumentId = request.InvestInstrumentId,
                Quantity = request.Quantity
            };
            _repository.Add(entity);
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
