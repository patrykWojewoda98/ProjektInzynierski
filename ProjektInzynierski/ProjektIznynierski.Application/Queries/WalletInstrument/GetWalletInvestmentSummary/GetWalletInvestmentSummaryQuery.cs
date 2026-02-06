using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.WalletInstrument.GetWalletInvestmentSummary
{
    public class GetWalletInvestmentSummaryQuery
        : IRequest<WalletSummaryDto>
    {
        public int WalletId { get; }

        public GetWalletInvestmentSummaryQuery(int walletId)
        {
            WalletId = walletId;
        }
    }
}
