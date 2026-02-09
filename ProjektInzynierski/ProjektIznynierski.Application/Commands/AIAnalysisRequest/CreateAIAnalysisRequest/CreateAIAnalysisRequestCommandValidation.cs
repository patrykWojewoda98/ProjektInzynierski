using FluentValidation;

namespace ProjektIznynierski.Application.Commands.AIAnalysisRequest.CreateAIAnalysisRequest
{
    public class CreateAIAnalysisRequestCommandValidation : AbstractValidator<CreateAIAnalysisRequestCommand>
    {
        public CreateAIAnalysisRequestCommandValidation()
        {
            RuleFor(x => x.InvestInstrumentId)
                .GreaterThan(0)
                .WithMessage("InvestInstrumentId must be greater than zero.");

            RuleFor(x => x.ClientId)
                .GreaterThan(0)
                .WithMessage("ClientId must be greater than zero.");
        }
    }
}
