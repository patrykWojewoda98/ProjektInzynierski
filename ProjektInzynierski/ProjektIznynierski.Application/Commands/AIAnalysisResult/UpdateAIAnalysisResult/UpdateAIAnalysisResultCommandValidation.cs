using FluentValidation;

namespace ProjektIznynierski.Application.Commands.AIAnalysisResult.UpdateAIAnalysisResult
{
    public class UpdateAIAnalysisResultCommandValidation : AbstractValidator<UpdateAIAnalysisResultCommand>
    {
        public UpdateAIAnalysisResultCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator wyniku analizy AI jest wymagany i musi być większy od zera.");

            RuleFor(x => x.Summary)
                .NotEmpty().WithMessage("Podsumowanie jest wymagane.")
                .MaximumLength(2000).WithMessage("Podsumowanie nie może przekraczać 2000 znaków.");

            RuleFor(x => x.Recommendation)
                .NotEmpty().WithMessage("Rekomendacja jest wymagana.")
                .MaximumLength(1000).WithMessage("Rekomendacja nie może przekraczać 1000 znaków.");

            RuleFor(x => x.KeyInsights)
                .NotEmpty().WithMessage("Kluczowe wnioski są wymagane.")
                .MaximumLength(3000).WithMessage("Kluczowe wnioski nie mogą przekraczać 3000 znaków.");

            RuleFor(x => x.ConfidenceScore)
                .GreaterThanOrEqualTo(0).When(x => x.ConfidenceScore.HasValue).WithMessage("Wskaźnik pewności nie może być ujemny.");

            RuleFor(x => x.ClientId)
                .GreaterThan(0).WithMessage("Identyfikator klienta jest wymagany i musi być większy od zera.");
        }
    }
}
