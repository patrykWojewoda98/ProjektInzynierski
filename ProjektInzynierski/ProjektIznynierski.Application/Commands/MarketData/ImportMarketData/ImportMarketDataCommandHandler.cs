using MediatR;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using System.Diagnostics;

namespace ProjektIznynierski.Application.Commands.MarketData.ImportMarketData
{
    internal class ImportMarketDataCommandHandler
        : IRequestHandler<ImportMarketDataCommand, int>
    {
        private readonly IMarketDataRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IYahooFinanceService _client;
        private readonly IInvestInstrumentRepository _instrumentRepository;

        public ImportMarketDataCommandHandler(IMarketDataRepository repository,IUnitOfWork unitOfWork, IYahooFinanceService client,IInvestInstrumentRepository instrumentRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _client = client;
            _instrumentRepository = instrumentRepository;
        }

        public async Task<int> Handle(ImportMarketDataCommand request,CancellationToken cancellationToken)
        {
            var instrument = await _instrumentRepository
                .GetByTickerAsync(request.Ticker, cancellationToken);
            Debug.WriteLine($"Importing market data for Ticker: {request.Ticker}");
            if (instrument == null)
                throw new ArgumentException(
                    $"Instrument with ISIN {request.Ticker} not found.");

            var snapshot = await _client.GetMarketDataByTIcker(
                request.Ticker,
                cancellationToken);

            Debug.WriteLine($"Fetched market data snapshot: {snapshot?.Date.ToShortDateString()},LowPirce: {snapshot?.LowPrice}");

            if (snapshot == null)
                return 0;

            if (await _repository.ExistsAsync(
                instrument.Id,
                snapshot.Date.Date,
                cancellationToken))
            {
                return 0;
            }

            var entity = new Domain.Entities.MarketData
            {
                InvestInstrumentId = instrument.Id,
                Date = snapshot.Date,

                OpenPrice = snapshot.OpenPrice ?? 0,
                ClosePrice = snapshot.ClosePrice ?? 0,
                HighPrice = snapshot.HighPrice ?? 0,
                LowPrice = snapshot.LowPrice ?? 0,
                Volume = snapshot.Volume ?? 0
            };

            _repository.Add(entity);

            await _unitOfWork.SaveChangesAsync();

            return 1;
        }
    }
}
