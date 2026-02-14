using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    public class PersonalReportPdfService : IPersonalReportPdfService
    {
        public byte[] GeneratePdf(
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
            int? fontSize = null)
        {
            var hasAnySection = includeInstrumentInfo || includeFinancialMetrics || includeFinancialReports
                || includePortfolioComposition;
            if (!hasAnySection)
                throw new Exception("At least one report section must be selected.");

            var effectiveFontSize = fontSize ?? 10;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x =>
                    {
                        x = x.FontSize(effectiveFontSize);
                        if (!string.IsNullOrWhiteSpace(fontFamily))
                            x = x.FontFamily(fontFamily);
                        return x;
                    });

                    page.Header().Element(e => BuildHeader(e, includeInstrumentInfo, instrument));

                    page.Content().Element(e => BuildContent(
                        e,
                        includeInstrumentInfo,
                        includeFinancialMetrics,
                        includedMetricFields,
                        includeFinancialReports,
                        includePortfolioComposition,
                        instrument,
                        metrics,
                        reports ?? Enumerable.Empty<FinancialReport>(),
                        wallet,
                        walletInstruments ?? new List<WalletInstrument>(),
                        customIntroText,
                        customOutroText));

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Personal Report • AI Investment Advisory System • ");
                        text.Span(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm")).SemiBold();
                    });
                });
            }).GeneratePdf();
        }

        private void BuildHeader(
            IContainer container,
            bool includeInstrumentInfo,
            InvestInstrument? instrument)
        {
            container.Column(col =>
            {
                col.Item().Text("Personal Investment Report").FontSize(18).Bold();
                col.Item().PaddingVertical(4);

                if (includeInstrumentInfo && instrument != null)
                {
                    col.Item().Text(instrument.Name).FontSize(14).SemiBold();
                    col.Item().Text($"Ticker: {instrument.Ticker} | ISIN: {instrument.Isin ?? "—"}");
                }

                col.Item().PaddingVertical(6);
                col.Item().LineHorizontal(1);
            });
        }

        private void BuildContent(
            IContainer container,
            bool includeInstrumentInfo,
            bool includeFinancialMetrics,
            IReadOnlyList<string>? includedMetricFields,
            bool includeFinancialReports,
            bool includePortfolioComposition,
            InvestInstrument? instrument,
            FinancialMetric? metrics,
            IEnumerable<FinancialReport> reports,
            Wallet? wallet,
            List<WalletInstrument> walletInstruments,
            string? customIntroText = null,
            string? customOutroText = null)
        {
            container.Column(col =>
            {
                if (!string.IsNullOrWhiteSpace(customIntroText))
                {
                    col.Item().Text(customIntroText);
                    col.Item().PaddingVertical(10);
                }

                if (includeInstrumentInfo && instrument != null)
                {
                    col.Item().Text("Instrument details").Bold().FontSize(12);
                    col.Item().Text($"Name: {instrument.Name}");
                    col.Item().Text($"Ticker: {instrument.Ticker}");
                    col.Item().Text($"ISIN: {instrument.Isin ?? "—"}");
                    if (!string.IsNullOrEmpty(instrument.Description))
                        col.Item().Text($"Description: {instrument.Description}");
                    col.Item().PaddingVertical(10);
                }

                if (includeFinancialMetrics && metrics != null)
                {
                    col.Item().Text("Financial metrics").Bold().FontSize(12);
                    var parts = new List<string>();
                    var fields = includedMetricFields ?? new[] { "PE", "PB", "ROE", "DebtToEquity", "DividendYield" };
                    if (fields.Contains("PE", StringComparer.OrdinalIgnoreCase)) parts.Add($"PE: {metrics.PE ?? 0}");
                    if (fields.Contains("PB", StringComparer.OrdinalIgnoreCase)) parts.Add($"PB: {metrics.PB ?? 0}");
                    if (fields.Contains("ROE", StringComparer.OrdinalIgnoreCase)) parts.Add($"ROE: {metrics.ROE ?? 0}%");
                    if (fields.Contains("DebtToEquity", StringComparer.OrdinalIgnoreCase)) parts.Add($"Debt/Equity: {metrics.DebtToEquity ?? 0}");
                    if (fields.Contains("DividendYield", StringComparer.OrdinalIgnoreCase)) parts.Add($"Dividend Yield: {metrics.DividendYield ?? 0}%");
                    col.Item().Text(parts.Count > 0 ? string.Join(" | ", parts) : "No metrics selected.");
                    col.Item().PaddingVertical(10);
                }

                if (includeFinancialReports)
                {
                    col.Item().Text("Financial reports").Bold().FontSize(12);
                    if (reports == null || !reports.Any())
                        col.Item().Text("No financial reports available.");
                    else
                    {
                        foreach (var r in reports)
                        {
                            col.Item()
                                .Background(Colors.Grey.Lighten3)
                                .Padding(6)
                                .Column(rcol =>
                                {
                                    rcol.Item().Text($"Period: {r.Period}").Bold();
                                    rcol.Item().Text(
                                        $"Revenue: {r.Revenue} | Net Income: {r.NetIncome} | EPS: {r.EPS}");
                                    rcol.Item().Text(
                                        $"Assets: {r.Assets} | Liabilities: {r.Liabilities}");
                                    rcol.Item().Text(
                                        $"Operating Cash Flow: {r.OperatingCashFlow} | Free Cash Flow: {r.FreeCashFlow}");
                                });
                            col.Item().PaddingBottom(6);
                        }
                    }
                    col.Item().PaddingVertical(10);
                }

                if (includePortfolioComposition)
                {
                    col.Item().Text("Portfolio composition").Bold().FontSize(12);
                    if (wallet == null || walletInstruments == null || !walletInstruments.Any())
                        col.Item().Text("No portfolio data available.");
                    else
                    {
                        col.Item().Text($"Cash balance: {wallet.CashBalance}").SemiBold();
                        foreach (var wi in walletInstruments)
                        {
                            var name = wi.InvestInstrument?.Name ?? $"Instrument #{wi.InvestInstrumentId}";
                            col.Item()
                                .Background(Colors.Grey.Lighten3)
                                .Padding(6)
                                .Text($"{name}: Quantity {wi.Quantity}");
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(customOutroText))
                {
                    col.Item().PaddingVertical(10);
                    col.Item().Text(customOutroText);
                }
            });
        }

    }
}
