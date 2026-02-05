using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Pdf
{
    public class GenerateInvestmentRecommendationPdfCommandHandler
        : IRequestHandler<GenerateInvestmentRecommendationPdfCommand, byte[]>
    {
        private readonly IAIAnalisisRequestRepository _analysisRequestRepository;
        private readonly IInvestInstrumentRepository _investInstrumentRepository;
        private readonly IFinancialMetricRepository _financialMetricRepository;
        private readonly IFinancialReportRepository _financialReportRepository;
        private readonly IInvestmentRecommendationPdfService _pdfService;

        public GenerateInvestmentRecommendationPdfCommandHandler(
            IAIAnalisisRequestRepository analysisRequestRepository,
            IInvestInstrumentRepository investInstrumentRepository,
            IFinancialMetricRepository financialMetricRepository,
            IFinancialReportRepository financialReportRepository,
            IInvestmentRecommendationPdfService pdfService)
        {
            _analysisRequestRepository = analysisRequestRepository;
            _investInstrumentRepository = investInstrumentRepository;
            _financialMetricRepository = financialMetricRepository;
            _financialReportRepository = financialReportRepository;
            _pdfService = pdfService;
        }

        public async Task<byte[]> Handle(
            GenerateInvestmentRecommendationPdfCommand request,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Analiza AI
            var analysisRequest = await _analysisRequestRepository
                .GetByIdAsync(request.AnalysisRequestId, cancellationToken);

            if (analysisRequest == null)
                throw new Exception("AI analysis request not found");

            if (analysisRequest.AIAnalysisResult == null)
                throw new Exception("AI analysis result not found");

            // 2️⃣ Instrument inwestycyjny
            var instrument = await _investInstrumentRepository
                .GetByIdAsync(analysisRequest.InvestInstrumentId, cancellationToken);

            if (instrument == null)
                throw new Exception("InvestInstrument not found");

            // 3️⃣ Dane finansowe
            var metrics = await _financialMetricRepository
                .GetByInvestInstrumentIdAsync(
                    analysisRequest.InvestInstrumentId,
                    cancellationToken);

            var reports = await _financialReportRepository
                .GetByInstrumentIdAsync(
                    analysisRequest.InvestInstrumentId,
                    cancellationToken);

            // 4️⃣ Generowanie PDF
            return _pdfService.GeneratePdf(
                analysisRequest,
                instrument,
                metrics,
                reports);
        }
    }
}
