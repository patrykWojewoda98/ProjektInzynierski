using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.WalletInstrument.GetWalletInvestmentSummary;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Wallet.GetWalletInvestmentSummary;

internal class GetWalletInvestmentSummaryQueryHandler
    : IRequestHandler<GetWalletInvestmentSummaryQuery, List<WalletInvestmentSummaryDto>>
{
    private readonly IWalletInstrumentRepository _repository;

    public GetWalletInvestmentSummaryQueryHandler(
        IWalletInstrumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<WalletInvestmentSummaryDto>> Handle(
        GetWalletInvestmentSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByWalletIdWithInstrumentAsync(request.WalletId, cancellationToken);

        return entities
            .GroupBy(x => x.InvestInstrumentId)
            .Select(group => new WalletInvestmentSummaryDto
            {
                InstrumentId = group.Key,
                InstrumentName = group.First().InvestInstrument.Name,
                TotalQuantity = group.Sum(x => x.Quantity)
            })
            .ToList();
    }
}
