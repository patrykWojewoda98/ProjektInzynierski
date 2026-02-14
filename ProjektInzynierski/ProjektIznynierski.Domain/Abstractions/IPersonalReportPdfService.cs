using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IPersonalReportPdfService
    {
        byte[] GeneratePdf(
            bool includeInstrumentInfo,
            bool includeFinancialMetrics,
            IReadOnlyList<string>? includedMetricFields,
            bool includeFinancialReports,
            bool includePortfolioComposition,
            InvestInstrument? instrument,
            FinancialMetric? metrics,
            IEnumerable<FinancialReport> reports,
            Wallet? wallet,
            List<WalletInstrument>? walletInstruments,
            string? customIntroText = null,
            string? customOutroText = null,
            string? fontFamily = null,
            int? fontSize = null);
    }
}
