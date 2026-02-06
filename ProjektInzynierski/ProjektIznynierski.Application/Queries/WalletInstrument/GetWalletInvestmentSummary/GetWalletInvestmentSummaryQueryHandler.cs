using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Application.Queries.WalletInstrument.GetWalletInvestmentSummary;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;

internal class GetWalletInvestmentSummaryQueryHandler
    : IRequestHandler<GetWalletInvestmentSummaryQuery, WalletSummaryDto>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletInstrumentRepository _walletInstrumentRepository;
    private readonly ICurrencyPairRepository _currencyPairRepository;
    private readonly ICurrencyRateHistoryRepository _rateHistoryRepository;
    private readonly IMarketDataRepository _marketDataRepository;

    public GetWalletInvestmentSummaryQueryHandler(
        IWalletRepository walletRepository,
        IWalletInstrumentRepository walletInstrumentRepository,
        ICurrencyPairRepository currencyPairRepository,
        ICurrencyRateHistoryRepository rateHistoryRepository,
        IMarketDataRepository marketDataRepository)
    {
        _walletRepository = walletRepository;
        _walletInstrumentRepository = walletInstrumentRepository;
        _currencyPairRepository = currencyPairRepository;
        _rateHistoryRepository = rateHistoryRepository;
        _marketDataRepository = marketDataRepository;
    }

    public async Task<WalletSummaryDto> Handle(
        GetWalletInvestmentSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdWithCurrencyAsync(request.WalletId);
        if (wallet == null)
            throw new Exception("Wallet not found");

        var accountCurrencyId = wallet.CurrencyId;
        var accountCurrencyName = wallet.Currency.Name;

        var walletInstruments =
            await _walletInstrumentRepository.GetByWalletIdWithInstrumentAsync(
                request.WalletId, cancellationToken);

        var grouped = walletInstruments.GroupBy(x => x.InvestInstrumentId);

        var investments = new List<WalletInvestmentSummaryDto>();
        decimal totalInvestmentsValue = 0;

        foreach (var group in grouped)
        {
            var instrument = group.First().InvestInstrument;
            var quantity = group.Sum(x => x.Quantity);

            var marketData =
                await _marketDataRepository.GetLatestByInvestInstrumentIdAsync(
                    instrument.Id, cancellationToken);

            if (marketData == null)
                throw new Exception($"No market data for {instrument.Name}");

            var pricePerUnit = marketData.ClosePrice;
            var instrumentValue = quantity * pricePerUnit;

            decimal valueInAccountCurrency;

            if (instrument.CurrencyId == accountCurrencyId)
            {
                valueInAccountCurrency = instrumentValue;
            }
            else
            {
                var pair = await _currencyPairRepository
                    .GetByCurrenciesAsync(
                        instrument.CurrencyId,
                        accountCurrencyId);

                CurrencyRateHistory rateHistory;

                if (pair != null)
                {
                    rateHistory =
                        await _rateHistoryRepository.GetLatestRateAsync(pair.Id);
                    valueInAccountCurrency = instrumentValue * rateHistory.CloseRate;
                }
                else
                {
                    var reversePair = await _currencyPairRepository
                        .GetByCurrenciesAsync(
                            accountCurrencyId,
                            instrument.CurrencyId);

                    if (reversePair == null)
                        throw new Exception("Currency pair not found");

                    rateHistory =
                        await _rateHistoryRepository.GetLatestRateAsync(reversePair.Id);

                    valueInAccountCurrency = instrumentValue / rateHistory.CloseRate;
                }
            }

            totalInvestmentsValue += valueInAccountCurrency;

            investments.Add(new WalletInvestmentSummaryDto
            {
                InstrumentId = instrument.Id,
                InstrumentName = instrument.Name,
                TotalQuantity = (int)quantity,
                InstrumentCurrency = instrument.Currency.Name,
                PricePerUnit = pricePerUnit,
                ValueInAccountCurrency = valueInAccountCurrency
            });
        }

        return new WalletSummaryDto
        {
            AccountCurrency = accountCurrencyName,
            CashBalance = wallet.CashBalance,
            TotalInvestmentsValue = totalInvestmentsValue,
            Investments = investments
        };
    }
}
