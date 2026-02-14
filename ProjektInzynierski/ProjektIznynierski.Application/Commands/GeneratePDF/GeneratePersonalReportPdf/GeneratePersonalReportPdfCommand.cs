using MediatR;

namespace ProjektIznynierski.Application.Commands.Pdf
{
    public record GeneratePersonalReportPdfCommand(
        int ClientId,
        int? InvestInstrumentId,
        bool IncludeInstrumentInfo,
        bool IncludeFinancialMetrics,
        IReadOnlyList<string>? IncludedMetricFields,
        bool IncludeFinancialReports,
        IReadOnlyList<int>? IncludedFinancialReportIds,
        bool IncludePortfolioComposition,
        string? CustomIntroText,
        string? CustomOutroText,
        string? FontFamily,
        int? FontSize
    ) : IRequest<byte[]>;
}
