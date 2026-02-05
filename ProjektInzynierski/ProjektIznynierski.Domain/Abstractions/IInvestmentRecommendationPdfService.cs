using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IInvestmentRecommendationPdfService
    {
        byte[] GeneratePdf(AIAnalysisRequest analysisRequest,InvestInstrument instrument,FinancialMetric metrics,IEnumerable<FinancialReport> reports);
    }
}
