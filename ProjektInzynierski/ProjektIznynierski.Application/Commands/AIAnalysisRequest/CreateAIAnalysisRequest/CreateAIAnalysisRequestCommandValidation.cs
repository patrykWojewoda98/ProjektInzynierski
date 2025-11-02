using FluentValidation;

namespace ProjektIznynierski.Application.Commands.AIAnalysisRequest.CreateAIAnalysisRequest
{
    public class CreateAIAnalysisRequestCommandValidation : AbstractValidator<CreateAIAnalysisRequestCommand>
    {
        public CreateAIAnalysisRequestCommandValidation()
        {
            RuleFor(x => x.FinancialReportId)
                .GreaterThan(0).WithMessage("Identyfikator raportu finansowego jest wymagany i musi być większy od zera.");

            RuleFor(x => x.InvestProfileId)
                .GreaterThan(0).WithMessage("Identyfikator profilu inwestycyjnego jest wymagany i musi być większy od zera.");
        }
    }
}
