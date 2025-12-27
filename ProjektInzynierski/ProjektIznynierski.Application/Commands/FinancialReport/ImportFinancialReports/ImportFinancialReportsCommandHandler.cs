using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.FinancialReport.ImportFinancialReports
{
    internal class ImportFinancialReportsCommandHandler
        : IRequestHandler<ImportFinancialReportsCommand, int>
    {
        private readonly IFinancialReportRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStrefaInwestorowClientService _client;
        private readonly IInvestInstrumentRepository _instrumentRepository;

        public ImportFinancialReportsCommandHandler(
            IFinancialReportRepository repository,
            IUnitOfWork unitOfWork,
            IStrefaInwestorowClientService client,
            IInvestInstrumentRepository instrumentRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _client = client;
            _instrumentRepository = instrumentRepository;
        }

        public async Task<int> Handle(
            ImportFinancialReportsCommand request,
            CancellationToken cancellationToken)
        {
            var instrument = await _instrumentRepository
            .GetByIsinAsync(request.Isin, cancellationToken);

            if (instrument == null)
                throw new ArgumentException($"Instrument with ISIN {request.Isin} not found.");
            // Pobierz snapshoty 
            var snapshots = await _client.GetFinancialReportsAsync(
                request.Isin,
                cancellationToken
            );

            if (snapshots.Count == 0)
                return 0;

            //Mapuj snapshot → encja domenowa
            foreach (var snap in snapshots)
            {
                var entity = new Domain.Entities.FinancialReport
                {
                    InvestInstrumentId = instrument.Id,
                    Period = snap.Period,
                    Revenue = snap.Revenue,
                    NetIncome = snap.NetIncome,
                    EPS = snap.EPS,
                    Assets = snap.Assets,
                    Liabilities = snap.Liabilities,
                    OperatingCashFlow = snap.OperatingCashFlow,
                    FreeCashFlow = snap.FreeCashFlow
                };
                //Sprawdź czy encja już istnieje
                if (await _repository.ExistsAsync(
                    entity.InvestInstrumentId,
                    entity.Period,
                    cancellationToken))
                {
                    continue;
                }
                _repository.Add(entity);
            }

            await _unitOfWork.SaveChangesAsync();
            return snapshots.Count;
        }
    }
}

