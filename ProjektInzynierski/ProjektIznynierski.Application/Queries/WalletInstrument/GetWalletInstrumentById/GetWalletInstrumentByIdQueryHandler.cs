using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.WalletInstrument.GetWalletInstrumentById
{
    internal class GetWalletInstrumentsByWalletIdQueryHandler: IRequestHandler<GetWalletInstrumentsByWalletIdQuery, List<WalletInstrumentDto>>
    {
        private readonly IWalletRepository _walletRepository;

        public GetWalletInstrumentsByWalletIdQueryHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<List<WalletInstrumentDto>> Handle(
            GetWalletInstrumentsByWalletIdQuery request,
            CancellationToken cancellationToken)
        {
            var instruments = await _walletRepository
                .GetWalletInstrumentsByWalletIdAsync(request.WalletId);

            return instruments.Select(wi => new WalletInstrumentDto
            {
                Id = wi.Id,
                WalletId = wi.WalletId,
                InvestInstrumentId = wi.InvestInstrumentId,
                Quantity = wi.Quantity,
            }).ToList();
        }
    }
}
