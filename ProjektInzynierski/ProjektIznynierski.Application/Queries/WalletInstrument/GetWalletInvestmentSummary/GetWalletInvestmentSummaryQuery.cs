using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.WalletInstrument.GetWalletInvestmentSummary
{
    public record GetWalletInvestmentSummaryQuery(int WalletId) : IRequest<List<WalletInvestmentSummaryDto>>;

}
