using FluentValidation;

namespace ProjektIznynierski.Application.Commands.AIAnalysisRequest.UpdateAIAnalysisRequest
{
    public class UpdateAIAnalysisRequestCommandValidation : AbstractValidator<UpdateAIAnalysisRequestCommand>
    {
        public UpdateAIAnalysisRequestCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator żądania analizy AI jest wymagany i musi być większy od zera.");

            RuleFor(x => x.FinancialReportId)
                .GreaterThan(0).WithMessage("Identyfikator raportu finansowego jest wymagany i musi być większy od zera.");

            RuleFor(x => x.InvestProfileId)
                .GreaterThan(0).WithMessage("Identyfikator profilu inwestycyjnego jest wymagany i musi być większy od zera.");
        }
    }
}
