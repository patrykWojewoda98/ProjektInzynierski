using MediatR;

namespace ProjektIznynierski.Application.Commands.Pdf
{
    public record GenerateInvestmentRecommendationPdfCommand(int AnalysisRequestId) : IRequest<byte[]>;
}
