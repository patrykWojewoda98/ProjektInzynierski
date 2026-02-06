using MediatR;
using ProjektInzynierski.Domain.Models;
using ProjektIznynierski.Application.Queries.WalletInstrument.GetWalletInvestmentSummary;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Wallet.ExportWalletToExcel
{
    internal class ExportWalletToExcelCommandHandler
        : IRequestHandler<ExportWalletToExcelCommand, byte[]>
    {
        private readonly IMediator _mediator;
        private readonly IWalletRepository _walletRepository;
        private readonly IXlsxService _xlsxService;

        public ExportWalletToExcelCommandHandler(
            IMediator mediator,
            IWalletRepository walletRepository,
            IXlsxService xlsxService)
        {
            _mediator = mediator;
            _walletRepository = walletRepository;
            _xlsxService = xlsxService;
        }

        public async Task<byte[]> Handle(
            ExportWalletToExcelCommand request,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Pobierz summary (SPRAWDZONA LOGIKA)
            var summary = await _mediator.Send(
                new GetWalletInvestmentSummaryQuery(request.WalletId),
                cancellationToken);

            // 2️⃣ Pobierz wallet (cash + waluta)
            var wallet = await _walletRepository
                .GetByIdWithCurrencyAsync(request.WalletId);

            if (wallet == null)
                throw new Exception("Wallet not found");

            // 3️⃣ Zbuduj snapshot
            var snapshot = new WalletSnapshot
            {
                WalletId = wallet.Id,
                AccountCurrency = summary.AccountCurrency,
                CashAmount = wallet.CashBalance,
                TotalValueInAccountCurrency =
                    wallet.CashBalance + summary.TotalInvestmentsValue,
                Instruments = summary.Investments.Select(i =>
                    new WalletInstrumentSnapshot
                    {
                        InstrumentName = i.InstrumentName,
                        Quantity = i.TotalQuantity,
                        UnitPrice = i.PricePerUnit,
                        InstrumentCurrency = i.InstrumentCurrency,
                        PositionValue =
                            i.PricePerUnit * i.TotalQuantity,
                        PositionValueInAccountCurrency =
                            i.ValueInAccountCurrency,
                    }).ToList()
            };

            // 4️⃣ Excel
            return _xlsxService.GenerateWalletExcel(snapshot);
        }
    }
}
