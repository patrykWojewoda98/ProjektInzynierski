using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.WalletInstrument.GetAllWalletInstruments
{
    public class GetAllWalletInstrumentsQueryHandler : IRequestHandler<GetAllWalletInstrumentsQuery, List<WalletInstrumentDto>>
    {
        private readonly IWalletInstrumentRepository _walletInstrumentRepository;

        public GetAllWalletInstrumentsQueryHandler(IWalletInstrumentRepository walletInstrumentRepository)
        {
            _walletInstrumentRepository = walletInstrumentRepository;
        }

        public async Task<List<WalletInstrumentDto>> Handle(GetAllWalletInstrumentsQuery request, CancellationToken cancellationToken)
        {
            var walletInstruments = await _walletInstrumentRepository.GetAllAsync(cancellationToken);
            
            return walletInstruments.Select(wi => new WalletInstrumentDto
            {
                Id = wi.Id,
                WalletId = wi.WalletId,
                InvestInstrumentId = wi.InvestInstrumentId,
                Quantity = wi.Quantity
            }).ToList();
        }
    }
}
