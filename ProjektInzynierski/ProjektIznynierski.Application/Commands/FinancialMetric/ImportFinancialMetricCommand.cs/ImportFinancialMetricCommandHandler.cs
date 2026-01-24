using MediatR;
using ProjektIznynierski.Domain.Abstractions;
namespace ProjektIznynierski.Application.Commands.FinancialMetric.ImportFinancialIndicators
{
    public class ImportFinancialMetricCommandHandler
        : IRequestHandler<ImportFinancialMetricCommand, int>
    {
        private readonly IFinancialMetricRepository _metricRepository;
        private readonly IInvestInstrumentRepository _instrumentRepository;
        private readonly IStrefaInwestorowClientService _client;
        private readonly IUnitOfWork _unitOfWork;

        public ImportFinancialMetricCommandHandler(IFinancialMetricRepository metricRepository,IInvestInstrumentRepository instrumentRepository,IStrefaInwestorowClientService client,IUnitOfWork unitOfWork)
        {
            _metricRepository = metricRepository;
            _instrumentRepository = instrumentRepository;
            _client = client;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(
            ImportFinancialMetricCommand request,
            CancellationToken cancellationToken)
        {
            var instrument = await _instrumentRepository
                .GetByIdAsync(request.InvestInstrumentId, cancellationToken);

            if (instrument == null)
                throw new InvalidOperationException(
                    $"InvestInstrument with ID {request.InvestInstrumentId} not found.");

            if (string.IsNullOrWhiteSpace(instrument.Isin))
                throw new InvalidOperationException(
                    $"InvestInstrument with ID {request.InvestInstrumentId} does not have ISIN.");

            var indicators = await _client
                .GetFinancialIndicatorsAsync(instrument.Isin, cancellationToken);

            var metric = await _metricRepository
                .GetByInvestInstrumentIdAsync(
                    request.InvestInstrumentId,
                    cancellationToken);

            if (metric == null)
            {
                metric = new Domain.Entities.FinancialMetric
                {
                    InvestmentInstrumentId = request.InvestInstrumentId,
                    DebtToEquity = indicators.DebtToEquity,
                    DividendYield = indicators.DividendYield,
                    PE = indicators.PE,
                    PB = indicators.PB,
                    ROE = indicators.ROE,
                    CreatedAt = DateTime.UtcNow
                };

                _metricRepository.Add(metric);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                instrument.FinancialMetricId = metric.Id;
                _instrumentRepository.Update(instrument);

            }
            else
            {
                metric.DebtToEquity = indicators.DebtToEquity;
                metric.DividendYield = indicators.DividendYield;
                metric.PE = indicators.PE;
                metric.PB = indicators.PB;
                metric.ROE = indicators.ROE;
                metric.UpdatedAt = DateTime.UtcNow;

                _metricRepository.Update(metric);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return metric.Id;
        }
    }
}
