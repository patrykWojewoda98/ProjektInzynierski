using MediatR;
using ProjektIznynierski.Domain.Abstractions;
using DomainEntities = ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Application.Commands.Pdf
{
    public class GeneratePersonalReportPdfCommandHandler : IRequestHandler<GeneratePersonalReportPdfCommand, byte[]>
    {
        private readonly IInvestInstrumentRepository _investInstrumentRepository;
        private readonly IFinancialMetricRepository _financialMetricRepository;
        private readonly IFinancialReportRepository _financialReportRepository;
        private readonly IAIAnalisisRequestRepository _analysisRequestRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletInstrumentRepository _walletInstrumentRepository;
        private readonly IPersonalReportPdfService _pdfService;

        public GeneratePersonalReportPdfCommandHandler(
            IInvestInstrumentRepository investInstrumentRepository,
            IFinancialMetricRepository financialMetricRepository,
            IFinancialReportRepository financialReportRepository,
            IAIAnalisisRequestRepository analysisRequestRepository,
            IWalletRepository walletRepository,
            IWalletInstrumentRepository walletInstrumentRepository,
            IPersonalReportPdfService pdfService)
        {
            _investInstrumentRepository = investInstrumentRepository;
            _financialMetricRepository = financialMetricRepository;
            _financialReportRepository = financialReportRepository;
            _analysisRequestRepository = analysisRequestRepository;
            _walletRepository = walletRepository;
            _walletInstrumentRepository = walletInstrumentRepository;
            _pdfService = pdfService;
        }

        public async Task<byte[]> Handle(GeneratePersonalReportPdfCommand request, CancellationToken cancellationToken)
        {
            DomainEntities.InvestInstrument? instrument = null;
            DomainEntities.FinancialMetric? metrics = null;
            IEnumerable<DomainEntities.FinancialReport> reports = Array.Empty<DomainEntities.FinancialReport>();
            DomainEntities.Wallet? wallet = null;
            List<DomainEntities.WalletInstrument>? walletInstruments = null;

            if (request.InvestInstrumentId.HasValue)
            {
                instrument = await _investInstrumentRepository.GetByIdAsync(request.InvestInstrumentId.Value, cancellationToken);
                if (instrument == null)
                    throw new Exception("Investment instrument not found.");

                if (request.IncludeFinancialMetrics)
                    metrics = await _financialMetricRepository.GetByInvestInstrumentIdAsync(request.InvestInstrumentId.Value, cancellationToken);

                if (request.IncludeFinancialReports)
                {
                    var allReports = await _financialReportRepository.GetByInstrumentIdAsync(request.InvestInstrumentId.Value, cancellationToken);
                    if (request.IncludedFinancialReportIds != null)
                    {
                        var idSet = request.IncludedFinancialReportIds.ToHashSet();
                        reports = allReports.Where(r => idSet.Contains(r.Id)).ToList();
                    }
                    else
                        reports = allReports;
                }
            }
            else if (request.IncludeInstrumentInfo || request.IncludeFinancialMetrics || request.IncludeFinancialReports)
            {
                throw new Exception("Investment instrument is required when including instrument info, financial metrics, or financial reports.");
            }

            if (request.IncludePortfolioComposition)
            {
                wallet = await _walletRepository.GetWalletByClientIdAsync(request.ClientId);
                if (wallet != null)
                    walletInstruments = (await _walletInstrumentRepository.GetByWalletIdWithInstrumentAsync(wallet.Id, cancellationToken)) ?? new List<DomainEntities.WalletInstrument>();
                else
                    walletInstruments = new List<DomainEntities.WalletInstrument>();
            }

            return _pdfService.GeneratePdf(
                request.IncludeInstrumentInfo,
                request.IncludeFinancialMetrics,
                request.IncludedMetricFields,
                request.IncludeFinancialReports,
                request.IncludePortfolioComposition,
                instrument,
                metrics,
                reports,
                wallet,
                walletInstruments);
        }
    }
}
