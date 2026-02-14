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
        bool IncludePortfolioComposition
    ) : IRequest<byte[]>;
}
